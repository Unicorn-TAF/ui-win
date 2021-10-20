using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

#pragma warning disable S3885 // "Assembly.Load" should be used
namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to run tests in specified order and with specified per suite categories.
    /// It is parameterized by dictionary where key is suite name and value is category
    /// </summary>
    public class PlaylistRunner : TestsRunner
    {
        private const string DataSetDelimiter = "::";

        private readonly string _testsAssemblyFile;
        private readonly Dictionary<string, string> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistRunner"/> class 
        /// for specified assembly and with specified filters.
        /// </summary>
        /// <param name="assemblyPath">path to tests assembly file</param>
        /// <param name="filters">filters (key: suite name, value: tests categories to run within the suite)</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public PlaylistRunner(string assemblyPath, Dictionary<string, string> filters) 
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException("Tests assembly not found.", assemblyPath);
            }

            _testsAssemblyFile = assemblyPath;
            Outcome = new LaunchOutcome();
            _filters = filters;
        }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        /// <exception cref="TypeLoadException">is thrown when suite class was not 
        /// found for specified suite name in run filters</exception>
        public override void RunTests()
        {
            var testsAssembly = Assembly.LoadFrom(_testsAssemblyFile);

            var suitesToRun = CollectSuitesToRun(testsAssembly);

            if (!suitesToRun.Any())
            {
                return;
            }

            Outcome.StartTime = DateTime.Now;

            // Execute run init action if exists in assembly.
            try
            {
                GetRunInitCleanupMethod(testsAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Error, "Run initialization failed:\n" + ex);
                Outcome.RunInitialized = false;
                Outcome.RunnerException = ex.InnerException;
            }

            if (Outcome.RunInitialized)
            {
                foreach (var suiteEntry in suitesToRun.Keys)
                {
                    string[] pair = Regex.Split(suiteEntry, DataSetDelimiter);
                    string dataSet = pair.Length == 2 ? pair[1] : string.Empty;

                    Config.SetTestCategories(_filters[suiteEntry]);

                    RunTestSuite(suitesToRun[suiteEntry], dataSet);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }
        }

        private void RunTestSuite(Type type, string dataSet)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                var dataSetsToRun = string.IsNullOrEmpty(dataSet) ?
                    AdapterUtilities.GetSuiteData(type) :
                    AdapterUtilities.GetSuiteData(type).Where(ds => ds.Name.Equals(dataSet));

                foreach (var parametersSet in dataSetsToRun)
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;
                    parameterizedSuite.Metadata.Add("Data Set", parametersSet.Name);
                    parameterizedSuite.Outcome.DataSetName = parametersSet.Name;
                    ExecuteSuiteIteration(parameterizedSuite);
                }
            }
            else
            {
                var suite = Activator.CreateInstance(type) as TestSuite;
                ExecuteSuiteIteration(suite);
            }
        }

        private Dictionary<string, Type> CollectSuitesToRun(Assembly assembly)
        {
            var suitesToRun = new Dictionary<string, Type>();
             
            //As suite entry in filter can contain data set name need to extract pure suites names
            //to filter assembly types by them 
            var suiteNames = _filters.Keys.Select(k => GetSuiteNameFromFilter(k));

            var filteredSuites = TestsObserver.ObserveTestSuites(assembly)
                .Where(s => suiteNames.Contains(GetSuiteNameByType(s)));

            foreach (var filterSuiteName in _filters.Keys)
            {
                var suiteName = GetSuiteNameFromFilter(filterSuiteName);

                var suite = filteredSuites.FirstOrDefault(
                    s => GetSuiteNameByType(s).Equals(suiteName, StringComparison.InvariantCultureIgnoreCase));

                if (suite == null)
                {
                    throw new TypeLoadException($"Suite with name '{suiteName}' was not found in tests assembly.");
                }

                if (suite.GetRuntimeMethods().Any(t => IsTestRunnable(t, _filters[filterSuiteName])))
                {
                    suitesToRun.Add(filterSuiteName, suite);
                }
            }

            return suitesToRun;
        }

        private bool IsTestRunnable(MethodInfo method, string category)
        {
            if (!method.IsDefined(typeof(TestAttribute), true) || method.IsDefined(typeof(DisabledAttribute), true))
            {
                return false;
            }

            if (string.IsNullOrEmpty(category))
            {
                return true;
            }
            else
            {
                return (from attribute
                    in method.GetCustomAttributes<CategoryAttribute>(true)
                    select attribute.Category.Trim())
                    .Contains(category, StringComparer.InvariantCultureIgnoreCase);
            }
        }

        private string GetSuiteNameByType(Type suiteType) =>
            suiteType.GetCustomAttribute<SuiteAttribute>(true).Name.Trim();

        private string GetSuiteNameFromFilter(string filterSuiteName) =>
            Regex.Split(filterSuiteName, DataSetDelimiter)[0]; 
    }
}
#pragma warning restore S3885 // "Assembly.Load" should be used
