using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteMethod : TestSuiteMethodBase
    {

        public bool IsBeforeSuite = false;

        public override string[] Categories
        {
            get
            {
                if (_categories == null)
                    _categories = new string[0];

                return _categories;
            }
        }

        // Events section

        public delegate void TestSuiteMethodEvent(TestSuiteMethod suiteMethod);

        public static event TestSuiteMethodEvent onStart;
        public static event TestSuiteMethodEvent onFinish;
        public static event TestSuiteMethodEvent onPass;
        public static event TestSuiteMethodEvent onFail;


        /// <summary>
        /// Object describing single test, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public TestSuiteMethod(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome();
        }


        /// <summary>
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public override void Execute(TestSuite suiteInstance)
        {
            CurrentOutput = new StringBuilder();
            try
            {
                onStart?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onStart event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Info($"========== {(IsBeforeSuite ? "BEFORE" : "AFTER")} SUITE '{Description}' ==========");

            TestTimer = new Stopwatch();

            TestTimer.Start();
            try
            {
                TestMethod.Invoke(suiteInstance, null);
                Outcome.Result = Result.PASSED;

                onPass?.Invoke(this);
            }
            catch (Exception ex)
            {
                Fail(ex.InnerException, suiteInstance.CurrentStepBug);
                onFail?.Invoke(this);
            }

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;

            Logger.Instance.Info($"{(IsBeforeSuite ? "BEFORE" : "AFTER")} SUITE {Outcome.Result}");

            try
            {
                onFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onFinish event invoke" + Environment.NewLine + ex);
            }
        }
    }
}
