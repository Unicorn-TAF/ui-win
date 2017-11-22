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

        public Guid Id;
        public Guid ParentId;

        public static StringBuilder CurrentOutput = new StringBuilder();


        private string _author = null;
        /// <summary>
        /// Test author. If author not specified through AuthorAttribute, then return "No author"
        /// </summary>
        public string Author
        {
            get
            {
                if (_author == null)
                {
                    object[] attributes = TestMethod.GetCustomAttributes(typeof(AuthorAttribute), true);

                    if (attributes.Length > 0)
                    {
                        AuthorAttribute attribute = (AuthorAttribute)attributes[0];
                        _author = attribute.Author;
                    }
                    else
                    {
                        _author = "No author";
                    }
                }
                return _author;
            }
        }


        private string _description = null;
        /// <summary>
        /// Test description. If description not specified through TestAttribute, then return test method name
        /// </summary>
        public string Description
        {
            get
            {
                if (_description == null)
                {
                    object[] attributes = TestMethod.GetCustomAttributes(typeof(TestAttribute), true);

                    if(attributes.Length > 0)
                    {
                        TestAttribute attribute = (TestAttribute)attributes[0];
                        _description = attribute.Description;
                    }

                    if (string.IsNullOrEmpty(_description))
                        _description = TestMethod.Name;
                }
                return _description;
            }
            set
            {
                _description = value;
            }
        }


        private string _fullTestName = null;
        /// <summary>
        /// Full test name which is "{Suite Name} - {test method name}"
        /// </summary>
        public string FullTestName
        {
            get
            {
                if (_fullTestName == null)
                    _fullTestName = TestMethod.Name;
                return _fullTestName;
            }
            set
            {
                _fullTestName = value;
            }
        }


        protected string[] _categories = null;
        /// <summary>
        /// Test categories list. Test could not have any category
        /// </summary>
        public abstract string[] Categories { get; }


        /// <summary>
        /// Current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome;


        /// <summary>
        /// Test execution timer
        /// </summary>
        public Stopwatch TestTimer;


        //Method which represents test
        protected MethodInfo TestMethod;
        

        /// <summary>
        /// Generates Id for the test wich will be the same each time for this test
        /// </summary>
        public void GenerateId()
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(FullTestName));
                Id = new Guid(hash);
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
                Outcome.Bugs = bugs.Split(',');
            else
                Outcome.Bugs = new string[] { "?" };

            Outcome.Exception = ex;
            Outcome.Result = Result.FAILED;
        }
    }
}
