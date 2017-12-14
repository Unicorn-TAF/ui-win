using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public abstract class TestSuiteMethodBase
    {
        public static TimeSpan TestTimeout = TimeSpan.FromMinutes(15);
        public static StringBuilder CurrentOutput = new StringBuilder();

        public Guid Id;
        public Guid ParentId;

        protected string[] categories = null;

        // Method which represents test
        protected MethodInfo testMethod;

        private string author = null;
        private string description = null;
        private string fullTestName = null;

        /// <summary>
        /// Test execution timer
        /// </summary>
        protected Stopwatch testTimer;

        /// <summary>
        /// Gets test author. If author not specified through AuthorAttribute, then return "No author"
        /// </summary>
        public string Author
        {
            get
            {
                if (this.author == null)
                {
                    object[] attributes = this.testMethod.GetCustomAttributes(typeof(AuthorAttribute), true);

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
                    object[] attributes = this.testMethod.GetCustomAttributes(typeof(TestAttribute), true);

                    if (attributes.Length > 0)
                    {
                        TestAttribute attribute = (TestAttribute)attributes[0];
                        this.description = attribute.Description;
                    }

                    if (string.IsNullOrEmpty(this.description))
                    {
                        this.description = this.testMethod.Name;
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
        public string FullTestName
        {
            get
            {
                if (this.fullTestName == null)
                {
                    this.fullTestName = this.testMethod.Name;
                }
                    
                return this.fullTestName;
            }

            set
            {
                this.fullTestName = value;
            }
        }

        /// <summary>
        /// Gets test categories list. Test could not have any category
        /// </summary>
        public abstract string[] Categories { get; }

        /// <summary>
        /// Gets or sets Current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome
        {
            get;

            set;
        }

        /// <summary>
        /// Generates Id for the test wich will be the same each time for this test
        /// </summary>
        public void GenerateId()
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(this.FullTestName));
                this.Id = new Guid(hash);
            }
        }

        /// <summary>
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public abstract void Execute(TestSuite suiteInstance);

        /// <summary>
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        /// <param name="bugs">string of bugs test failed on current step.</param>
        protected void Fail(Exception ex, string bugs)
        {
            Logger.Instance.Error(ex.ToString());

            if (!string.IsNullOrEmpty(bugs))
            {
                this.Outcome.Bugs = bugs.Split(',');
            }
            else
            {
                this.Outcome.Bugs = new string[] { "?" };
            }

            this.Outcome.Exception = ex;
            this.Outcome.Result = Result.FAILED;
        }
    }
}
