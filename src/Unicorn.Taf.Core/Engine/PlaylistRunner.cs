using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unicorn.Taf.Api;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to run tests in specified order and with specified per suite categories.
    /// It is parameterized by dictionary where key is suite name and value is category
    /// </summary>
    public class PlaylistRunner : TestsRunner
    {
        private const string DataSetDelimiter = "::";

        private readonly Assembly _testAssembly;
        private readonly Dictionary<string, string> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaylistRunner"/> class 
        /// for specified assembly and with specified filters.
        /// </summary>
        /// <param name="testAssembly">assembly with tests</param>
        /// <param name="filters">filters (key: suite name, value: tests categories to run within the suite)</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public PlaylistRunner(Assembly testAssembly, Dictionary<string, string> filters) 
        {
            if (testAssembly == null)
            {
                throw new ArgumentNullException(nameof(testAssembly));
            }

            if (filters == null)
            {
                throw new ArgumentNullException(nameof(filters));
            }

            _testAssembly = testAssembly;
            Outcome = new LaunchOutcome();
            _filters = filters;
        }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        /// <exception cref="TypeLoadException">is thrown when suite class was not 
        /// found for specified suite name in run filters</exception>
        public override IOutcome RunTests()
        {
            var suitesToRun = CollectSuitesToRun(_testAssembly);

            if (!suitesToRun.Any())
            {
                return null;
            }

            Outcome.StartTime = DateTime.Now;

            // Execute run init action if exists in assembly.
            try
            {
                GetRunInitCleanupMethod(_testAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                ULog.Error("Run initialization failed: {0}", ex);
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
                GetRunInitCleanupMethod(_testAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }

            return Outcome;
        }

        private void RunTestSuite(Type type, string dataSet)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                List<DataSet> dataSetsToRun = string.IsNullOrEmpty(dataSet) ?
                    AdapterUtilities.GetSuiteData(type) :
                    AdapterUtilities.GetSuiteData(type).Where(ds => ds.Name.Equals(dataSet)).ToList();

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
            Dictionary<string, Type> suitesToRun = new Dictionary<string, Type>();
             
            //As suite entry in filter can contain data set name need to extract pure suites names
            //to filter assembly types by them 
            IEnumerable<string> suiteNames = _filters.Keys.Select(k => GetSuiteNameFromFilter(k));

            List<Type> filteredSuites = TestsObserver.ObserveTestSuites(assembly)
                .Where(s => suiteNames.Contains(AdapterUtilities.GetSuiteName(s)))
                .ToList();

            foreach (var filterSuiteName in _filters.Keys)
            {
                string suiteName = GetSuiteNameFromFilter(filterSuiteName);

                Type suite = filteredSuites.FirstOrDefault(s => 
                    AdapterUtilities.GetSuiteName(s).Equals(suiteName, StringComparison.InvariantCultureIgnoreCase));

                if (suite == null)
                {
                    throw new TypeLoadException($"Suite with name '{suiteName}' was not found in tests assembly.");
                }

                if (suite.IsDefined(typeof(DisabledAttribute)))
                {
                    continue;
                }

                if (suite.GetRuntimeMethods().Any(t => AdapterUtilities.IsTestRunnable(t, _filters[filterSuiteName])))
                {
                    suitesToRun.Add(filterSuiteName, suite);
                }
            }

            return suitesToRun;
        }

        private static string GetSuiteNameFromFilter(string filterSuiteName) =>
            Regex.Split(filterSuiteName, DataSetDelimiter)[0]; 
    }
}
