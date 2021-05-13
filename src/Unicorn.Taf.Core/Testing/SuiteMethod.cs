using System;
using System.Diagnostics;
using System.Reflection;
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
        /// <summary>
        /// Method executed before all suite tests.
        /// </summary>
        BeforeSuite,

        /// <summary>
        /// Method executed before each suite test.
        /// </summary>
        BeforeTest,

        /// <summary>
        /// Method executed after each suite test.
        /// </summary>
        AfterTest,

        /// <summary>
        /// Method executed after all suite tests.
        /// </summary>
        AfterSuite,

        /// <summary>
        /// Test itself.
        /// </summary>
        Test
    }

    /// <summary>
    /// Represents the suite method itself (which is also base for <see cref="Test"/>).
    /// Contains list of events related to different suite method states (started, finished, skipped, passed, failed)<para/>
    /// Contains methods to execute the suite method and base methods for all types of suite methods<para/>
    /// </summary>
    public class SuiteMethod
    {
        private const string NoAuthor = "No author";

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteMethod"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public SuiteMethod(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome
            {
                FullMethodName = AdapterUtilities.GetFullTestMethodName(testMethod),
            };

            var testAttribute = TestMethod
                .GetCustomAttribute(typeof(TestAttribute), true) as TestAttribute;

            var authorAttribute = TestMethod
                .GetCustomAttribute(typeof(AuthorAttribute), true) as AuthorAttribute;

            Outcome.Author = authorAttribute == null ? NoAuthor : authorAttribute.Author;
            Outcome.Id = AdapterUtilities.GuidFromString(Outcome.FullMethodName);

            Outcome.Title = string.IsNullOrEmpty(testAttribute?.Title) ?
                TestMethod.Name :
                testAttribute.Title;
        }

        /// <summary>
        /// Delegate used for suite method events invocation
        /// </summary>
        /// <param name="suiteMethod">current <see cref="SuiteMethod"/> instance</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
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
            Logger.Instance.Log(LogLevel.Info, $"---- {MethodType} '{Outcome.Title}'");

            try
            {
                OnSuiteMethodStart?.Invoke(this);
                RunSuiteMethod(suiteInstance);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, 
                    "Exception occured during " + nameof(OnSuiteMethodStart) + " event invoke" + Environment.NewLine + ex);
                Fail(ex.InnerException);
            }
            finally
            {
                try
                {
                    OnSuiteMethodFinish?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Warning,
                        "Exception occured during " + nameof(OnSuiteMethodFinish) + " event invoke" + Environment.NewLine + ex);
                }
            }

            LogStatus();
        }

        /// <summary>
        /// Fail test, fill <see cref="TestOutcome"/> exception and bugs and invoke onFail event.<para/>
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        public void Fail(Exception ex)
        {
            Logger.Instance.Log(LogLevel.Error, ex.ToString());

            Outcome.Exception = ex;
            Outcome.Result = Status.Failed;
        }

        protected void LogStatus()
        {
            LogLevel level;

            switch (Outcome.Result)
            {
                case Status.Failed:
                    level = LogLevel.Error;
                    break;
                case Status.Skipped:
                    level = LogLevel.Warning;
                    break;
                default:
                    level = LogLevel.Info;
                    break;
            }

            Logger.Instance.Log(level, $"{MethodType} {Outcome.Result}");
        }

        private void RunSuiteMethod(TestSuite suiteInstance)
        {
            Outcome.StartTime = DateTime.Now;
            TestTimer = Stopwatch.StartNew();

            try
            {
                var testTask = Task.Run(() =>
                {
                    TestMethod.Invoke(suiteInstance, null);
                });

                var restSuiteExecutionTime = Config.SuiteTimeout - suiteInstance.ExecutionTimer.Elapsed;

                if (restSuiteExecutionTime < TimeSpan.Zero)
                {
                    restSuiteExecutionTime = TimeSpan.Zero;
                }

                if (restSuiteExecutionTime <= Config.TestTimeout && !testTask.Wait(restSuiteExecutionTime))
                {
                    throw new SuiteTimeoutException($"Suite timeout ({Config.SuiteTimeout}) reached");
                }
                else if (!testTask.Wait(Config.TestTimeout))
                {
                    throw new TestTimeoutException($"{MethodType} timeout ({Config.TestTimeout}) reached");
                }
                
                Outcome.Result = Status.Passed;

                try
                {
                    OnSuiteMethodPass?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, 
                        "Exception occured during " + nameof(OnSuiteMethodPass) + " event invoke" + Environment.NewLine + e);
                }
            }
            catch (Exception ex)
            {
                var failExeption = ex is TestTimeoutException || ex is SuiteTimeoutException ?
                    ex :
                    ex.InnerException.InnerException;

                Fail(failExeption);

                try
                {
                    OnSuiteMethodFail?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Warning, 
                        "Exception occured during " + nameof(OnSuiteMethodFail) + " event invoke" + Environment.NewLine + e);
                }
            }

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;
        }
    }
}
