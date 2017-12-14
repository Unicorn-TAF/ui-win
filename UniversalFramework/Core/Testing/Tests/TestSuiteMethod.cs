using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteMethod : TestSuiteMethodBase
    {
        private bool isBeforeSuite = false;
                
        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuiteMethod"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public TestSuiteMethod(MethodInfo testMethod)
        {
            this.testMethod = testMethod;
            this.Outcome = new TestOutcome();
        }

        /* Events section*/

        public delegate void TestSuiteMethodEvent(TestSuiteMethod suiteMethod);

        public static event TestSuiteMethodEvent OnStart;

        public static event TestSuiteMethodEvent OnFinish;

        public static event TestSuiteMethodEvent OnPass;

        public static event TestSuiteMethodEvent OnFail;

        public bool IsBeforeSuite
        {
            get
            {
                return this.isBeforeSuite;
            }

            set
            {
                this.isBeforeSuite = value;
            }
        }

        public override string[] Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new string[0];
                }

                return this.categories;
            }
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
                OnStart?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onStart event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Info($"========== {(IsBeforeSuite ? "BEFORE" : "AFTER")} SUITE '{Description}' ==========");

            this.testTimer = new Stopwatch();

            this.testTimer.Start();
            try
            {
                this.testMethod.Invoke(suiteInstance, null);
                this.Outcome.Result = Result.PASSED;

                OnPass?.Invoke(this);
            }
            catch (Exception ex)
            {
                Fail(ex.InnerException, suiteInstance.CurrentStepBug);
                OnFail?.Invoke(this);
            }

            this.testTimer.Stop();
            this.Outcome.ExecutionTime = this.testTimer.Elapsed;

            Logger.Instance.Info($"{(IsBeforeSuite ? "BEFORE" : "AFTER")} SUITE {Outcome.Result}");

            try
            {
                OnFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onFinish event invoke" + Environment.NewLine + ex);
            }
        }
    }
}
