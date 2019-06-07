using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

#pragma warning disable S3885 // "Assembly.Load" should be used
namespace Unicorn.Taf.Core.Engine
{
    public class TestsRunner
    {
        private readonly string testsAssemblyFile;

        protected TestsRunner()
        {
        }

        public TestsRunner(string assemblyPath) : this(assemblyPath, true)
        {
        }

        public TestsRunner(string assemblyPath, string configurationFileName)
        {
            this.testsAssemblyFile = assemblyPath;
            Config.FillFromFile(configurationFileName);
            this.Outcome = new LaunchOutcome();
        }

        public TestsRunner(string assemblyPath, bool getConfigFromFile)
        {
            this.testsAssemblyFile = assemblyPath;

            if (getConfigFromFile)
            {
                Config.FillFromFile(string.Empty);
            }

            this.Outcome = new LaunchOutcome();
        }

        public LaunchOutcome Outcome { get; protected set; }

        public virtual void RunTests()
        {
            var testsAssembly = Assembly.LoadFrom(this.testsAssemblyFile);

            var runnableSuites = TestsObserver.ObserveTestSuites(testsAssembly)
                .Where(s => AdapterUtilities.IsSuiteRunnable(s));

            if (!runnableSuites.Any())
            {
                return;
            }

            this.Outcome.StartTime = DateTime.Now;

            // Execute run init action if exists in assembly.
            try
            {
                GetRunInitCleanupMethod(testsAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Error, "Run initialization failed:\n" + ex);
                this.Outcome.RunInitialized = false;
                this.Outcome.RunnerException = ex.InnerException;
            }

            if (this.Outcome.RunInitialized)
            {
                foreach (var suiteType in runnableSuites)
                {
                    RunTestSuite(suiteType);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }
        }

        protected void RunTestSuite(Type type)
        {
            if (AdapterUtilities.IsSuiteParameterized(type))
            {
                var fullSuiteName = new StringBuilder();

                foreach (var parametersSet in AdapterUtilities.GetSuiteData(type))
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;
                    parameterizedSuite.Metadata.Add("Data Set", parametersSet.Name);

                    fullSuiteName.Clear().Append(parameterizedSuite.Name).Append($" [{parametersSet.Name}]");

                    parameterizedSuite.Name = fullSuiteName.ToString();
                    ExecuteSuiteIteration(parameterizedSuite);
                }
            }
            else
            {
                var suite = Activator.CreateInstance(type) as TestSuite;
                ExecuteSuiteIteration(suite);
            }
        }

        protected void ExecuteSuiteIteration(TestSuite testSuite)
        {
            testSuite.Execute();
            this.Outcome.SuitesOutcomes.Add(testSuite.Outcome);
        }

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