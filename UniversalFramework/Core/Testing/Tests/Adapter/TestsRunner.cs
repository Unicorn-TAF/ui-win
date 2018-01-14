using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class TestsRunner
    {
        private List<Type> runnableSuites;
        Assembly ass;

        public TestsRunner(Assembly ass)
        {
            this.ass = ass;
            Configuration.FillFromFile();
            ObserveRunnableSuites();
        }

        public delegate void UnicornTestEvent(Test test);
        


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

            var suite = Activator.CreateInstance(type) as TestSuite;
            ExecuteSuiteIteration(suite);
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
