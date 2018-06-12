using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class Test : SuiteMethod
    {
        private List<string> categories = null;
        private DataSet dataSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public Test(MethodInfo testMethod) : base(testMethod)
        {
            this.dataSet = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class, which is part of some TestSuite, based on specified <see cref="DataSet"/>.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        /// <param name="dataSet">DataSet to populate test method parameters; null if method does not have parameters</param>
        public Test(MethodInfo testMethod, DataSet dataSet) : base(testMethod)
        {
            this.dataSet = dataSet;
        }

        /* Events section */
        public delegate void TestEvent(Test test);

        public static event TestEvent OnStart;

        public static event TestEvent OnFinish;

        public static event TestEvent OnPass;

        public static event TestEvent OnFail;

        public static event TestEvent OnSkip;

        /// <summary>
        /// Gets test categories
        /// </summary>
        public List<string> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    this.categories = new List<string>();
                    var attributes = this.TestMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[];

                    foreach (var attribute in attributes)
                    {
                        this.categories.Add(attribute.Category.ToUpper().Trim());
                    }
                }

                return this.categories;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current test is need to be run
        /// </summary>
        public bool IsRunnable { get; set; } = true;

        /// <summary>
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public override void Execute(TestSuite suiteInstance)
        {
            SuiteMethod.CurrentOutput = new StringBuilder();

            try
            {
                OnStart?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during OnStart event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Info($"========== TEST '{Description}' ==========");

            this.TestTimer = new Stopwatch();
            this.TestTimer.Start();

            try
            {
                if (this.dataSet == null)
                {
                    this.TestMethod.Invoke(suiteInstance, null);
                }
                else
                {
                    this.TestMethod.Invoke(suiteInstance, this.dataSet.Parameters.ToArray());
                }
                
                this.Outcome.Result = Result.Passed;

                try
                {
                    OnPass?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Error("Exception occured during OnPass event invoke" + Environment.NewLine + e);
                }
            }
            catch (Exception ex)
            {
                Fail(ex.InnerException, suiteInstance.CurrentStepBug);

                try
                {
                    OnFail?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Error("Exception occured during OnFail event invoke" + Environment.NewLine + e);
                }
            }

            this.TestTimer.Stop();
            this.Outcome.ExecutionTime = this.TestTimer.Elapsed;

            Logger.Instance.Info($"TEST {Outcome.Result}");

            try
            {
                OnFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onFinish event invoke" + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Skip test and invoke onSkip event
        /// </summary>
        public void Skip()
        {
            this.Outcome.Result = Result.Skipped;
            this.Outcome.Bugs.Clear();
            OnSkip?.Invoke(this);
            Logger.Instance.Info($"TEST '{Description}' {Outcome.Result}");
        }

        /// <summary>
        /// Execute list of MethodInfo from TestSuite instance based on Field nameof TestSuite class.
        /// Used to run BeforeTests and AfterTests
        /// </summary>
        /// <param name="suiteInstance">instance of TestSuite</param>
        /// <param name="methodsList">name of Field contained array of MethodInfo</param>
        private void ExecuteMethods(TestSuite suiteInstance, string methodsList)
        {
            object field = typeof(TestSuite)
                .GetField(methodsList, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suiteInstance);

            MethodInfo[] methods = field as MethodInfo[];

            foreach (MethodInfo method in methods)
            {
                method.Invoke(suiteInstance, null);
            }
        }
    }
}
