using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Tests.Attributes;

namespace Unicorn.Taf.Core.Testing.Tests
{
    public class Test : SuiteMethod
    {
        private readonly DataSet dataSet;
        private List<string> categories = null;

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
            var postfix = $" [{dataSet.Name}]";
            this.dataSet = dataSet;

            this.Outcome.Title += postfix;
            this.Outcome.Id = GenerateId(this.Outcome.FullMethodName + postfix);
        }

        /* Events section */
        public delegate void TestEvent(Test test);

        public static event TestEvent OnTestStart;

        public static event TestEvent OnTestFinish;

        public static event TestEvent OnTestPass;

        public static event TestEvent OnTestFail;

        public static event TestEvent OnTestSkip;

        /// <summary>
        /// Gets test categories
        /// </summary>
        public List<string> Categories
        {
            get
            {
                if (this.categories == null)
                {
                    var attributes = this.TestMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[];

                    this.categories = new List<string>(
                        attributes.Select(a => a.Category.ToUpper().Trim())
                        .Where(c => !string.IsNullOrEmpty(c)));
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
            Logger.Instance.Log(LogLevel.Info, $"========== TEST '{this.Outcome.Title}' ==========");

            try
            {
                OnTestStart?.Invoke(this);
                this.RunTestMethod(suiteInstance);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnTestStart event invoke" + Environment.NewLine + ex);
                this.Skip("OnTestStart event failed");
            }
            finally
            {
                try
                {
                    OnTestFinish?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnTestFinish event invoke" + Environment.NewLine + ex);
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"TEST {Outcome.Result}");
        }

        /// <summary>
        /// Skip test and invoke OnTestSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        public void Skip(string reason)
        {
            this.Outcome.Result = Status.Skipped;

            try
            {
                OnTestSkip?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnTestSkip event invoke" + Environment.NewLine + e);
            }
        }

        private void RunTestMethod(TestSuite suiteInstance)
        {
            LogOutput.Clear();
            this.TestTimer = Stopwatch.StartNew();

            try
            {
                this.TestMethod.Invoke(suiteInstance, this.dataSet?.Parameters.ToArray());
                this.Outcome.Result = Status.Passed;

                try
                {
                    OnTestPass?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnTestPass event invoke" + Environment.NewLine + e);
                }
            }
            catch (Exception ex)
            {
                this.Fail(ex.InnerException);

                try
                {
                    OnTestFail?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnTestFail event invoke" + Environment.NewLine + e);
                }
            }

            this.TestTimer.Stop();
            this.Outcome.ExecutionTime = this.TestTimer.Elapsed;
            this.Outcome.Output = LogOutput.ToString();
            LogOutput.Clear();
        }
    }
}
