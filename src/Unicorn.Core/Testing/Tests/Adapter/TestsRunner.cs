using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class TestsRunner
    {
        private readonly Assembly testsAssembly;
        private readonly string testsAssemblyPath;

        public TestsRunner(Assembly assembly) : this (assembly, true)
        {
        }

        public TestsRunner(Assembly assembly, bool getConfigFromFile)
        {
            this.testsAssembly = assembly;

            if (getConfigFromFile)
            {
                Configuration.FillFromFile();
            }
        }

        public TestsRunner(Assembly assembly, string configurationFileName)
        {
            this.testsAssembly = assembly;
            Configuration.FillFromFile(configurationFileName);
        }

        public TestsRunner(string assemblyPath, string configurationFileName)
        {
            this.testsAssemblyPath = assemblyPath;
            Configuration.FillFromFile(configurationFileName);
        }

        public List<TestSuite> ExecutedSuites { get; protected set; }

        public Result RunStatus { get; protected set; }

        public void RunTests()
        {
            var testsDomain = CreateTestsDomain();

            this.ExecutedSuites = new List<TestSuite>();
            var runnableSuites = ObserveRunnableSuites();

            if (runnableSuites.Any())
            {
                // Execute run init action if exists in assembly.
                GetRunInitCleanupMethod(typeof(RunInitializeAttribute))?.Invoke(null, null);

                foreach (var suiteType in runnableSuites)
                {
                    RunTestSuite(suiteType);
                }

                // Execute run finalize action if exists in assembly.
                GetRunInitCleanupMethod(typeof(RunFinalizeAttribute))?.Invoke(null, null);

                this.RunStatus = this.ExecutedSuites
                    .Any(s => s.Outcome.Result.Equals(Result.Failed) || s.Outcome.Result.Equals(Result.Skipped)) ?
                    Result.Failed :
                    Result.Passed;
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
            this.ExecutedSuites.Add(testSuite);
        }

        private List<Type> ObserveRunnableSuites() =>
            TestsObserver.ObserveTestSuites(testsAssembly)
                .Where(s => AdapterUtilities.IsSuiteRunnable(s)).ToList();

        private MethodInfo GetRunInitCleanupMethod(Type attributeType)
        {
            var suitesWithRunInit = testsAssembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TestsAssemblyAttribute), true).Length > 0)
                .Where(s => GetTypeStaticMethodsWithAttribute(s, attributeType).Any());

            return suitesWithRunInit.Any() ?
                GetTypeStaticMethodsWithAttribute(suitesWithRunInit.First(), attributeType).First() :
                null;
        }

        private IEnumerable<MethodInfo> GetTypeStaticMethodsWithAttribute(Type containerType, Type attributeType)
        {
            return containerType.GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(attributeType, true) != null);
        }
    }
}
