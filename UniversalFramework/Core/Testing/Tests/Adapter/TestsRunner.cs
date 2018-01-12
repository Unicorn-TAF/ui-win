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
        Assembly ass;

        public TestsRunner(Assembly ass)
        {
            this.ass = ass;
            this.Configuration = Configuration.FromFile();
            ObserveRunnableSuites();
        }

        public delegate void UnicornTestEvent(Test test);
        public delegate void UnicornSuiteEvent(TestSuite testSuite);
        public delegate void UnicornSuiteMethodEvent(TestSuiteMethod testSuite);

        public static event UnicornSuiteEvent SuiteStarted;

        public static event UnicornSuiteEvent SuiteFinished;

        public static event UnicornSuiteEvent SuitePassed;

        public static event UnicornSuiteEvent SuiteFailed;

        ////public static event UnicornSuiteEvent SuiteSkipped;

        public static event UnicornSuiteMethodEvent SuiteMethodStarted;

        public static event UnicornSuiteMethodEvent SuiteMethodFinished;

        public static event UnicornSuiteMethodEvent SuiteMethodPassed;

        public static event UnicornSuiteMethodEvent SuiteMethodFailed;

        public static event UnicornTestEvent TestStarted;

        public static event UnicornTestEvent TestFinished;

        public static event UnicornTestEvent TestPassed;

        public static event UnicornTestEvent TestFailed;

        ////public static event UnicornTestEvent TestSkipped;

        public Configuration Configuration { get; set; }


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
                }
            }

            var suite = Activator.CreateInstance(type) as TestSuite;
            suite.Run();
        }

        private void RunTest(Test test)
        {

        }


        private void ObserveRunnableSuites()
        {
            runnableSuites = TestsObserver.ObserveTestSuites(ass)
                .Where(s => Util.IsSuiteRunnable(s, this.Configuration)).ToList();
        }
    }
}
