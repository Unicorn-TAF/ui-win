using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class TestsRunner
    {
        private List<Type> runnableSuites;
        private Assembly ass;

        public TestsRunner(Assembly ass, bool getConfigFromFile = true)
        {
            this.ass = ass;

            if (getConfigFromFile)
            {
                Configuration.FillFromFile();
            }

            this.ExecutedSuites = new List<TestSuite>();
            runnableSuites = ObserveRunnableSuites();
        }

        public TestsRunner(Assembly ass, string configurationFileName)
        {
            this.ass = ass;

            Configuration.FillFromFile(configurationFileName);

            this.ExecutedSuites = new List<TestSuite>();
            runnableSuites = ObserveRunnableSuites();
        }

        public List<TestSuite> ExecutedSuites { get; }

        public Result RunStatus { get; protected set; }

        public void RunTests()
        {
            if (runnableSuites.Any())
            {
                var initRun = GetRunInitCleanupMethods(typeof(RunInitializeAttribute));
                if (initRun != null)
                {
                    initRun.Invoke(null, null);
                }

                foreach (var suiteType in runnableSuites)
                {
                    RunTestSuite(suiteType);
                }

                var finalyzeRun = GetRunInitCleanupMethods(typeof(RunFinalizeAttribute));
                if (finalyzeRun != null)
                {
                    finalyzeRun.Invoke(null, null);
                }

                this.RunStatus = this.ExecutedSuites.Any(s => s.Outcome.Result.Equals(Result.Failed)) ? Result.Failed : Result.Passed;
            }
        }

        public void RunTestSuite(Type type)
        {
            if (Helper.IsSuiteParameterized(type))
            {
                foreach (var parametersSet in Helper.GetSuiteData(type))
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;
                    parameterizedSuite.Metadata.Add("Data Set", parametersSet.Name);
                    parameterizedSuite.Name += $" [{parametersSet.Name}]";
                    ExecuteSuiteIteration(parameterizedSuite);
                }
            }
            else
            {
                var suite = Activator.CreateInstance(type) as TestSuite;
                ExecuteSuiteIteration(suite);
            }
        }

        private TestSuite ExecuteSuiteIteration(TestSuite testSuite)
        {
            testSuite.Execute();
            this.ExecutedSuites.Add(testSuite);
            return testSuite;
        }

        private List<Type> ObserveRunnableSuites()
        {
            return TestsObserver.ObserveTestSuites(ass)
                .Where(s => Helper.IsSuiteRunnable(s)).ToList();
        }

        private MethodInfo GetRunInitCleanupMethods(Type attributeType)
        {
            var suitesWithRunInit = ass.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TestsAssemblyAttribute), true).Length > 0)
                .Where(s => GetTypeStaticMethodsWithAttribute(s, attributeType).Any());

            if (suitesWithRunInit.Any())
            {
                return GetTypeStaticMethodsWithAttribute(suitesWithRunInit.First(), attributeType).First();
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<MethodInfo> GetTypeStaticMethodsWithAttribute(Type containerType, Type attributeType)
        {
            return containerType.GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(attributeType, true) != null);
        }
    }
}
