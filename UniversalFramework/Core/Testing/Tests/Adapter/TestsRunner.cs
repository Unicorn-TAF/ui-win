using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        public List<TestSuite> ExecutedSuites { get; protected set; }

        public void RunTests()
        {
            foreach (var suiteType in runnableSuites)
            {
                RunTestSuite(suiteType);
            }
        }

        public void RunTestSuite(Type type)
        {
            if (Helper.IsSuiteParameterized(type))
            {
                foreach (var parametersSet in Helper.GetSuiteData(type))
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters.ToArray()) as TestSuite;
                    parameterizedSuite.Metadata.Add("postfix", parametersSet.Name);
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
    }
}
