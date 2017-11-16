using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteMethod : TestSuiteMethodBase
    {

        private string _description = null;
        /// <summary>
        /// Test description. If description not specified through TestAttribute, then return test method name
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                        _description = TestMethod.Name;
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public bool IsBeforeSuite = false;

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
            onStart?.Invoke(this);

            Logger.Instance.Info($"========== '{Description}' ==========");

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
            }

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;

            Logger.Instance.Info($"Suite method {Outcome.Result}");

            onFinish?.Invoke(this);
        }


        /// <summary>
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        /// <param name="bugs">string of bugs test failed on current step.</param>
        protected override void Fail(Exception ex, string bugs)
        {
            
            Logger.Instance.Error(ex.ToString());

            if (!string.IsNullOrEmpty(bugs))
                Outcome.Bugs = bugs.Split(',');
            else
                Outcome.Bugs = new string[] { "?" };

            Outcome.Exception = ex;
            Outcome.Result = Result.FAILED;
            onFail?.Invoke(this);
        }
    }
}
