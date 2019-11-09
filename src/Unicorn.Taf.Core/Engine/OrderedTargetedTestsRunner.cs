using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

#pragma warning disable S3885 // "Assembly.Load" should be used
namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides with ability to run tests in specified order and with specified per suite categories.
    /// It is parameterized by dictionary where key is suite name and value is category
    /// </summary>
    public class OrderedTargetedTestsRunner : TestsRunner
    {
        private readonly string _testsAssemblyFile;
        private readonly Dictionary<string, string> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderedTargetedTestsRunner"/> class for specified assembly and with specified filters.
        /// </summary>
        /// <param name="assemblyPath">path to tests assembly file</param>
        /// <param name="filters">filters (key: suite name, value: tests categories to run within the suite)</param>
        public OrderedTargetedTestsRunner(string assemblyPath, Dictionary<string, string> filters) 
        {
            _testsAssemblyFile = assemblyPath;
            Outcome = new LaunchOutcome();
            _filters = filters;
        }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        public override void RunTests()
        {
            var testsAssembly = Assembly.LoadFrom(_testsAssemblyFile);
            var orderedRunnableSuites = new List<Type>();
            var filteredSuites = TestsObserver.ObserveTestSuites(testsAssembly)
                .Where(s => _filters.Keys.Contains(GetSuiteName(s)));

            foreach (var suiteName in _filters.Keys)
            {
                var suite = filteredSuites
                    .First(s => GetSuiteName(s).Equals(suiteName, StringComparison.InvariantCultureIgnoreCase));

                if (suite.GetRuntimeMethods().Any(t => IsTestRunnable(t, _filters[suiteName])))
                {
                    orderedRunnableSuites.Add(suite);
                }
            }

            if (!orderedRunnableSuites.Any())
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
                foreach (var suiteType in orderedRunnableSuites)
                {
                    Config.SetTestCategories(_filters[GetSuiteName(suiteType)]);
                    RunTestSuite(suiteType);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }
        }

        private bool IsTestRunnable(MethodInfo method, string category)
        {
            if (method.GetCustomAttribute<TestAttribute>(true) == null || method.GetCustomAttribute<DisabledAttribute>(true) != null)
            {
                return false;
            }

            var categories = 
                from attribute
                in method.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[]
                select attribute.Category.ToUpper().Trim();

            return string.IsNullOrEmpty(category) || categories.Contains(category.ToUpper());
        }

        private string GetSuiteName(Type suiteType) =>
            (suiteType.GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute).Name.Trim();
    }
}
#pragma warning restore S3885 // "Assembly.Load" should be used
