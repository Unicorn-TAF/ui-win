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
            
            ObserveRunnableSuites();
        }

        public void RunTests()
        {
            foreach (var suiteType in runnableSuites)
            {
                RunTestSuite(suiteType);
            }
        }

        public void RunTestSuite(Type type)
        {
            if (Util.IsSuiteParameterized(type))
            {
                foreach (var parametersSet in Util.GetSuiteData(type))
                {
                    var parameterizedSuite = Activator.CreateInstance(type, parametersSet.Parameters) as TestSuite;
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

            return testSuite;
        }

        private void ObserveRunnableSuites()
        {
            runnableSuites = TestsObserver.ObserveTestSuites(ass)
                .Where(s => Util.IsSuiteRunnable(s)).ToList();
        }
    }
}
