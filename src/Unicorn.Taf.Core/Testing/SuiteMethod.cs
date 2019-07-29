using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents suite method type
    /// </summary>
    public enum SuiteMethodType
    {
        BeforeSuite,
        BeforeTest,
        AfterTest,
        AfterSuite,
        Test
    }

    /// <summary>
    /// Represents the suite method itself (which is also base for <see cref="Test"/>).
    /// Contains list of events related to different suite method states (started, finished, skipped, passed, failed)<para/>
    /// Contains methods to execute the suite method and base methods for all types of suite methods<para/>
    /// </summary>
    public class SuiteMethod
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteMethod"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public SuiteMethod(MethodInfo testMethod)
        {
            this.TestMethod = testMethod;
            this.Outcome = new TestOutcome
            {
                FullMethodName = AdapterUtilities.GetFullTestMethodName(testMethod),
            };

            var testAttribute = this.TestMethod
                .GetCustomAttribute(typeof(TestAttribute), true) as TestAttribute;

            var authorAttribute = this.TestMethod
                .GetCustomAttribute(typeof(AuthorAttribute), true) as AuthorAttribute;

            this.Outcome.Author = authorAttribute == null ? "No author" : authorAttribute.Author;
            this.Outcome.Id = AdapterUtilities.GuidFromString(this.Outcome.FullMethodName);

            this.Outcome.Title = string.IsNullOrEmpty(testAttribute?.Title) ?
                this.TestMethod.Name :
                testAttribute.Title;
        }

        /// <summary>
        /// Delegate used for suite method events invocation
        /// </summary>
        /// <param name="suiteMethod">current <see cref="SuiteMethod"/> instance</param>
        public delegate void UnicornSuiteMethodEvent(SuiteMethod suiteMethod);

        /// <summary>
        /// Event is invoked before suite method execution
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        /// <summary>
        /// Event is invoked after suite method execution
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        /// <summary>
        /// Event is invoked on suite method pass (<see cref="OnSuiteMethodFinish"/> OnTestFinish will be invoked anyway)
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        /// <summary>
        /// Event is invoked on suite method fail (<see cref="OnSuiteMethodFinish"/> will be invoked anyway)
        /// </summary>
        public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

        /// <summary>
        /// Gets current log in for of <see cref="StringBuilder"/>
        /// </summary>
        public static StringBuilder LogOutput { get; } = new StringBuilder();

        /// <summary>
        /// Gets or sets current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome { get; set; }

        /// <summary>
        /// Gets or sets current suite method type
        /// </summary>
        public SuiteMethodType MethodType { get; set; }

        /// <summary>
        /// Gets or sets <see cref="MethodInfo"/> which represents test
        /// </summary>
        public MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Gets or sets test method execution timer
        /// </summary>
        protected Stopwatch TestTimer { get; set; }

        /// <summary>
        /// Execute current test and fill <see cref="TestOutcome"/><para/>
        /// Before the test List of BeforeTests is executed<para/>
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public virtual void Execute(TestSuite suiteInstance)
        {
            Logger.Instance.Log(LogLevel.Info, $"========== {this.MethodType} '{this.Outcome.Title}' ==========");

            try
            {
                OnSuiteMethodStart?.Invoke(this);
                this.RunSuiteMethod(suiteInstance);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteMethodStart event invoke" + Environment.NewLine + ex);
                this.Fail(ex.InnerException);
            }
            finally
            {
                try
                {
                    OnSuiteMethodFinish?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteMethodFinish event invoke" + Environment.NewLine + ex);
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"{this.MethodType} {Outcome.Result}");
        }

        /// <summary>
        /// Fail test, fill <see cref="TestOutcome"/> exception and bugs and invoke onFail event.<para/>
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        public void Fail(Exception ex)
        {
            Logger.Instance.Log(LogLevel.Error, ex.ToString());

            this.Outcome.Exception = ex;
            this.Outcome.Result = Status.Failed;
        }

        private void RunSuiteMethod(TestSuite suiteInstance)
        {
            LogOutput.Clear();
            this.Outcome.StartTime = DateTime.Now;
            this.TestTimer = Stopwatch.StartNew();

            try
            {
                var testTask = Task.Run(() =>
                {
                    this.TestMethod.Invoke(suiteInstance, null);
                });

                if (!testTask.Wait(Config.TestTimeout))
                {
                    throw new TestTimeoutException($"{this.MethodType} timeout ({Config.TestTimeout}) reached");
                }
                
                this.Outcome.Result = Status.Passed;

                try
                {
                    OnSuiteMethodPass?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteMethodPass event invoke" + Environment.NewLine + e);
                }
            }
            catch (Exception ex)
            {
                this.Fail(ex is TestTimeoutException ? ex : ex.InnerException.InnerException);

                try
                {
                    OnSuiteMethodFail?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteMethodFail event invoke" + Environment.NewLine + e);
                }
            }

            this.TestTimer.Stop();
            this.Outcome.ExecutionTime = this.TestTimer.Elapsed;
            this.Outcome.Output = LogOutput.ToString();
            LogOutput.Clear();
        }
    }
}
