using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
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
        private string author = null;
        private string description = null;
        private string fullTestName = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteMethod"/> class, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public SuiteMethod(MethodInfo testMethod)
        {
            this.TestMethod = testMethod;
            this.Outcome = new TestOutcome();
        }

        /* Events section*/
        public delegate void UnicornSuiteMethodEvent(SuiteMethod suiteMethod);

        public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        /// <summary>
        /// Gets test author. If author not specified through AuthorAttribute, then return "No author"
        /// </summary>
        public string Author
        {
            get
            {
                if (this.author == null)
                {
                    var attribute = this.TestMethod.GetCustomAttribute(typeof(AuthorAttribute), true) as AuthorAttribute;

                    this.author = attribute != null ? attribute.Author : "No author";
                }

                return this.author;
            }
        }

        /// <summary>
        /// Gets or sets test description. If description not specified through TestAttribute, then return test method name
        /// </summary>
        public string Description
        {
            get
            {
                if (this.description == null)
                {
                    var attribute = this.TestMethod.GetCustomAttribute(typeof(TestAttribute), true) as TestAttribute;

                    if (attribute != null)
                    {
                        this.description = attribute.Description;
                    }

                    if (string.IsNullOrEmpty(this.description))
                    {
                        this.description = this.TestMethod.Name;
                    }
                }

                return this.description;
            }

            set
            {
                this.description = value;
            }
        }

        /// <summary>
        /// Gets or sets full test name which is "{Suite Name} - {test method name}"
        /// </summary>
        public string FullName
        {
            get
            {
                return this.fullTestName ?? (this.fullTestName = this.TestMethod.Name);
            }

            set
            {
                this.fullTestName = value;
            }
        }

        /// <summary>
        /// Gets or sets Current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome { get; set; }

        public SuiteMethodType MethodType { get; set; }

        /// <summary>
        /// Gets or sets Method which represents test
        /// </summary>
        public MethodInfo TestMethod { get; set; }

        /// <summary>
        /// Gets or sets Test execution timer
        /// </summary>
        protected Stopwatch TestTimer { get; set; }

        /// <summary>
        /// Generates Id for the test which will be the same each time for this test
        /// </summary>
        public void GenerateId()
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(this.FullName));
                this.Id = new Guid(hash);
            }
        }

        /// <summary>
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public virtual void Execute(TestSuite suiteInstance)
        {
            Logger.Instance.Log(LogLevel.Info, $"========== {this.MethodType} '{Description}' ==========");

            try
            {
                OnSuiteMethodStart?.Invoke(this);
                this.RunSuiteMethod(suiteInstance);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteMethodStart event invoke" + Environment.NewLine + ex);
                this.Fail(ex.InnerException, string.Empty);
            }
            finally
            {
                try
                {
                    OnSuiteMethodFinish?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteMethodFinish event invoke" + Environment.NewLine + ex);
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"{this.MethodType} {Outcome.Result}");
        }

        /// <summary>
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        /// <param name="bugs">string of bugs test failed on current step.</param>
        public void Fail(Exception ex, string bugs)
        {
            Logger.Instance.Log(LogLevel.Error, ex.ToString());

            this.Outcome.Bugs.Clear();

            if (!string.IsNullOrEmpty(bugs))
            {
                this.Outcome.Bugs.AddRange(bugs.Split(','));
            }
            else
            {
                this.Outcome.Bugs.Add("?");
            }

            this.Outcome.Exception = ex;
            this.Outcome.Result = Status.Failed;
        }

        private void RunSuiteMethod(TestSuite suiteInstance)
        {
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
                    Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteMethodPass event invoke" + Environment.NewLine + e);
                }
            }
            catch (Exception ex)
            {
                this.Fail(ex.InnerException, suiteInstance.CurrentStepBug);

                try
                {
                    OnSuiteMethodFail?.Invoke(this);
                }
                catch (Exception e)
                {
                    Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteMethodFail event invoke" + Environment.NewLine + e);
                }
            }

            this.TestTimer.Stop();
            this.Outcome.ExecutionTime = this.TestTimer.Elapsed;
        }
    }
}
