using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class TestsRunner
    {
        private List<Type> runnableSuites;

        public TestsRunner()
        {
            this.Configuration = Configuration.FromFile();
        }

        public delegate void UnicornTestEvent(Test test);
        public delegate void UnicornSuiteEvent(TestSuite testSuite);
        public delegate void UnicornSuiteMethodEvent(TestSuiteMethod testSuite);

        public static event UnicornTestEvent TestStarted;

        public static event UnicornTestEvent TestFinished;

        public static event UnicornTestEvent TestPassed;

        public static event UnicornTestEvent TestFailed;

        public static event UnicornTestEvent TestSkipped;

        /// <summary>
        /// Event raised on TestSuite start
        /// </summary>
        public static event UnicornSuiteEvent SuiteStarted;

        /// <summary>
        /// Event raised on TestSuite finish
        /// </summary>
        public static event UnicornSuiteEvent SuiteFinished;

        public static event UnicornSuiteEvent SuitePassed;

        public static event UnicornSuiteEvent SuiteFailed;

        /// <summary>
        /// Event raised on TestSuite skip
        /// </summary>
        public static event UnicornSuiteEvent SuiteSkipped;

        public static event UnicornSuiteMethodEvent SuiteMethodStarted;

        public static event UnicornSuiteMethodEvent SuiteMethodFinished;

        public static event UnicornSuiteMethodEvent SuiteMethodPassed;

        public static event UnicornSuiteMethodEvent SuiteMethodFailed;

        public Configuration Configuration { get; set; }


        private bool IsSuiteRunnable(Type suiteType)
        {
            bool runnable = true;

            var attributes = suiteType.GetType().GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[];
            List<string> features = (from attribute in attributes select attribute.Feature.ToUpper()).ToList();
            runnable &= features.Intersect(this.Configuration.RunFeatures).Count() > 0;


            return false;
        }
    }
}
