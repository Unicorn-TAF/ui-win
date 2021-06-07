using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

#pragma warning disable S3885 // "Assembly.Load" should be used
namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides ability to run tests which are filtered based on <see cref="Config"/>.
    /// </summary>
    public class TestsRunner
    {
        private readonly string _testsAssemblyFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly
        /// </summary>
        /// <param name="assemblyPath">path to tests assembly file</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public TestsRunner(string assemblyPath) : this(assemblyPath, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly based on specified configuration file
        /// </summary>
        /// <param name="assemblyPath">path to tests assembly file</param>
        /// <param name="configurationFileName">path to configuration file</param>
        /// <exception cref="FileNotFoundException">is thrown when tests assembly was not found</exception>
        public TestsRunner(string assemblyPath, string configurationFileName)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            if (configurationFileName == null)
            {
                throw new ArgumentNullException(nameof(configurationFileName));
            }

            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException("Tests assembly not found.", assemblyPath);
            }

            _testsAssemblyFile = assemblyPath;
            Config.FillFromFile(configurationFileName);
            Outcome = new LaunchOutcome();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class for specified assembly with ability to specify if need to load file from default config
        /// </summary>
        /// <param name="assemblyPath">path to tests assembly file</param>
        /// <param name="getConfigFromFile">true - if need to load config from default file <c>(.\unicorn.conf)</c>; false if use default values from <see cref="Config"/></param>
        public TestsRunner(string assemblyPath, bool getConfigFromFile)
        {
            if (assemblyPath == null)
            {
                throw new ArgumentNullException(nameof(assemblyPath));
            }

            _testsAssemblyFile = assemblyPath;

            if (getConfigFromFile)
            {
                Config.FillFromFile(string.Empty);
            }

            Outcome = new LaunchOutcome();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestsRunner"/> class.
        /// </summary>
        protected TestsRunner()
        {
        }

        /// <summary>
        /// Gets or sets launch outcome
        /// </summary>
        public LaunchOutcome Outcome { get; protected set; }

        /// <summary>
        /// Run all observed tests matching selection criteria
        /// </summary>
        public virtual void RunTests()
        {
            var testsAssembly = Assembly.LoadFrom(_testsAssemblyFile);

            var runnableSuites = TestsObserver.ObserveTestSuites(testsAssembly)
                .Where(s => AdapterUtilities.IsSuiteRunnable(s));

            if (!runnableSuites.Any())
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
                foreach (var suiteType in runnableSuites)
                {
                    RunTestSuite(suiteType);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }
        }

        /// <summary>
        /// Run test suite of specified <see cref="Type"/>
        /// </summary>
        /// <param name="type">suite <see cref="Type"/></param>
        protected void RunTestSuite(Type type)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                foreach (var parametersSet in AdapterUtilities.GetSuiteData(type))
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

        /// <summary>
        /// Execute suite iteration (the suite itself for non-parameterized, suite instance for current data set)
        /// </summary>
        /// <param name="testSuite">suite instance</param>
        protected void ExecuteSuiteIteration(TestSuite testSuite)
        {
            testSuite.Execute();
            Outcome.SuitesOutcomes.Add(testSuite.Outcome);
        }

        /// <summary>
        /// Get <see cref="MethodInfo"/> representing run initialization / cleanup
        /// </summary>
        /// <param name="assembly">assembly to search within</param>
        /// <param name="attributeType">Type of attribute class should be marked with</param>
        /// <returns><see cref="MethodInfo"/> instance</returns>
        protected MethodInfo GetRunInitCleanupMethod(Assembly assembly, Type attributeType)
        {
            var suitesWithRunInit = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TestAssemblyAttribute), true).Length > 0)
                .Where(s => GetTypeStaticMethodsWithAttribute(s, attributeType).Any());

            return suitesWithRunInit.Any() ?
                GetTypeStaticMethodsWithAttribute(suitesWithRunInit.First(), attributeType).First() :
                null;
        }

        private IEnumerable<MethodInfo> GetTypeStaticMethodsWithAttribute(Type containerType, Type attributeType) =>
            containerType.GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(attributeType, true) != null);
    }
}
#pragma warning restore S3885 // "Assembly.Load" should be used