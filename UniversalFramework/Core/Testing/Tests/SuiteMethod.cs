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

        public static event UnicornSuiteMethodEvent SuiteMethodStarted;

        public static event UnicornSuiteMethodEvent SuiteMethodFinished;

        public static event UnicornSuiteMethodEvent SuiteMethodPassed;

        public static event UnicornSuiteMethodEvent SuiteMethodFailed;

        public static TimeSpan TestTimeout => TimeSpan.FromMinutes(15);

        public static StringBuilder CurrentOutput { get; set; }

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
                    object[] attributes = this.TestMethod.GetCustomAttributes(typeof(AuthorAttribute), true);

                    if (attributes.Length > 0)
                    {
                        AuthorAttribute attribute = (AuthorAttribute)attributes[0];
                        this.author = attribute.Author;
                    }
                    else
                    {
                        this.author = "No author";
                    }
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
                    object[] attributes = this.TestMethod.GetCustomAttributes(typeof(TestAttribute), true);

                    if (attributes.Length > 0)
                    {
                        TestAttribute attribute = (TestAttribute)attributes[0];
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
                if (this.fullTestName == null)
                {
                    this.fullTestName = this.TestMethod.Name;
                }

                return this.fullTestName;
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
            CurrentOutput = new StringBuilder();

            try
            {
                SuiteMethodStarted?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during SuiteMethodStarted event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Info($"========== {this.MethodType} '{Description}' ==========");

            this.TestTimer = new Stopwatch();
            this.TestTimer.Start();

            try
            {
                this.TestMethod.Invoke(suiteInstance, null);
                this.Outcome.Result = Result.Passed;

                SuiteMethodPassed?.Invoke(this);
            }
            catch (Exception ex)
            {
                Fail(ex.InnerException, suiteInstance.CurrentStepBug);
                SuiteMethodFailed?.Invoke(this);
            }

            this.TestTimer.Stop();
            this.Outcome.ExecutionTime = this.TestTimer.Elapsed;

            Logger.Instance.Info($"{this.MethodType} {Outcome.Result}");

            try
            {
                SuiteMethodFinished?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during SuiteMethodFinished event invoke" + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        /// <param name="bugs">string of bugs test failed on current step.</param>
        protected void Fail(Exception ex, string bugs)
        {
            Logger.Instance.Error(ex.ToString());

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
            this.Outcome.Result = Result.Failed;
        }
    }
}
