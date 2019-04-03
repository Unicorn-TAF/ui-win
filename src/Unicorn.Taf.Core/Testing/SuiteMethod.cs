using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    public enum SuiteMethodType
    {
        BeforeSuite,
        BeforeTest,
        AfterTest,
        AfterSuite,
        Test
    }

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
            this.Outcome.Id = GenerateId(this.Outcome.FullMethodName);

            this.Outcome.Title = string.IsNullOrEmpty(testAttribute?.Title) ?
                this.TestMethod.Name :
                testAttribute.Title;
        }

        /* Events section*/
        public delegate void UnicornSuiteMethodEvent(SuiteMethod suiteMethod);

        public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

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
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
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
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        public void Fail(Exception ex)
        {
            Logger.Instance.Log(LogLevel.Error, ex.ToString());

            this.Outcome.Exception = ex;
            this.Outcome.Result = Status.Failed;
        }

        /// <summary>
        /// Generates Id for the test which will be the same each time for this test
        /// </summary>
        /// <param name="fullName">full test name including data set name</param>
        /// <returns>unique test method <see cref="Guid"/></returns>
        protected Guid GenerateId(string fullName)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(fullName));
                return new Guid(hash);
            }
        }

        private void RunSuiteMethod(TestSuite suiteInstance)
        {
            LogOutput.Clear();
            this.TestTimer = Stopwatch.StartNew();

            try
            {
                this.TestMethod.Invoke(suiteInstance, null);
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
                this.Fail(ex.InnerException);

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
