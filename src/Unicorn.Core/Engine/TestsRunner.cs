using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Engine
{
    public class TestsRunner
    {
        private readonly string testsAssemblyFile;

        public TestsRunner(string assemblyPath) : this(assemblyPath, true)
        {
        }

        public TestsRunner(string assemblyPath, string configurationFileName)
        {
            this.testsAssemblyFile = assemblyPath;
            Configuration.FillFromFile(configurationFileName);
            this.Outcome = new LaunchOutcome();
        }

        public TestsRunner(string assemblyPath, bool getConfigFromFile)
        {
            this.testsAssemblyFile = assemblyPath;

            if (getConfigFromFile)
            {
                Configuration.FillFromFile(string.Empty);
            }

            this.Outcome = new LaunchOutcome();
        }

        public LaunchOutcome Outcome { get; protected set; }

        public void RunTests()
        {
            var testsAssembly = Assembly.LoadFrom(this.testsAssemblyFile);

            var runnableSuites = TestsObserver.ObserveTestSuites(testsAssembly)
                .Where(s => AdapterUtilities.IsSuiteRunnable(s));

            if (runnableSuites.Any())
            {
                // Execute run init action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunInitializeAttribute))?.Invoke(null, null);

                foreach (var suiteType in runnableSuites)
                {
                    RunTestSuite(suiteType);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(testsAssembly, typeof(RunFinalizeAttribute))?.Invoke(null, null);
            }
        }

        public void RunTestSuite(Type type)
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

        private void ExecuteSuiteIteration(TestSuite testSuite)
        {
            testSuite.Execute();
            this.Outcome.SuitesOutcomes.Add(testSuite.Outcome);
        }

        private MethodInfo GetRunInitCleanupMethod(Assembly assembly, Type attributeType)
        {
            var suitesWithRunInit = assembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TestsAssemblyAttribute), true).Length > 0)
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
