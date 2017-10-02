using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class Test
    {
        public static TimeSpan TestTimeout = TimeSpan.FromMinutes(15);


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
                    TestAttribute attribute = (TestAttribute)attributes[0];
                    _description = attribute.Description;

                    if (string.IsNullOrEmpty(_description))
                        _description = TestMethod.Name;
                }
                return _description;
            }
        }


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


        private string[] _categories = null;
        /// <summary>
        /// Test categories list. Test could not have any category
        /// </summary>
        public string[] Categories
        {
            get
            {
                if (_categories == null)
                {
                    object[] attributes = TestMethod.GetCustomAttributes(typeof(CategoryAttribute), true);
                    if (attributes.Length != 0)
                    {
                        _categories = new string[attributes.Length];
                        for (int i = 0; i < attributes.Length; i++)
                            _categories[i] = ((CategoryAttribute)attributes[0]).Category.ToUpper().Trim();
                    }

                    else
                        _categories = new string[0];
                }
                return _categories;
            }
        }


        /// <summary>
        /// Indicate if specified test method should be skipped by presence of [Skip] attribute
        /// </summary>
        public bool IsNeedToBeSkipped;


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

        // Events section

        public delegate void TestEvent(Test test);

        public static event TestEvent onStart;
        public static event TestEvent onFinish;
        public static event TestEvent onPass;
        public static event TestEvent onFail;
        public static event TestEvent onSkip;


        /// <summary>
        /// Current test outcome, contains base information about execution results
        /// </summary>
        public TestOutcome Outcome;


        /// <summary>
        /// Test execution timer
        /// </summary>
        public Stopwatch TestTimer;


        //Method which represents test
        private MethodInfo TestMethod;
        


        public Test(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome();
            IsNeedToBeSkipped = false;
        }


        /// <summary>
        /// Execute current test and fill TestOutcome
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public void Execute(TestSuite suiteInstance)
        {
            if (IsNeedToBeSkipped)
            {
                Skip();
                return;
            }

            Logger.Instance.Info($"========== TEST '{Description}' ==========");

            onStart?.Invoke(this);

            TestTimer = new Stopwatch();

            TestTimer.Start();
            try
            {
                ExecuteMethods(suiteInstance, "ListBeforeTest");
                TestMethod.Invoke(suiteInstance, null);
                ExecuteMethods(suiteInstance, "ListAfterTest");
                Outcome.Result = Result.PASSED;

                onPass?.Invoke(this);
            }
            catch (Exception ex)
            {
                Fail(ex.InnerException, suiteInstance.CurrentStepBug);
            }

            TestTimer.Stop();
            Outcome.ExecutionTime = TestTimer.Elapsed;

            onFinish?.Invoke(this);

            Logger.Instance.Info($"Test {Outcome.Result}");
        }


        private void Skip()
        {
            Outcome.Result = Result.SKIPPED;
            onSkip?.Invoke(this);
            Logger.Instance.Info($"Test {Outcome.Result}");
        }


        private void Fail(Exception ex, string bugs)
        {
            Logger.Instance.Error(ex.ToString());

            if (!string.IsNullOrEmpty(bugs))
                Outcome.Bugs = bugs.Split(',');

            Outcome.Exception = ex;
            Outcome.Result = Result.FAILED;

            onFail?.Invoke(this);
        }


        public void CheckIfNeedToBeSkipped(params string[] categories)
        {
            object[] attributes = TestMethod.GetCustomAttributes(typeof(SkipAttribute), true);

            IsNeedToBeSkipped = attributes.Length != 0;

            if (categories.Length > 0)
                IsNeedToBeSkipped |= Categories.Intersect(categories).Count() != categories.Count();
        }


        private void ExecuteMethods(TestSuite suiteInstance, string methodsList)
        {
            object field = typeof(TestSuite)
                .GetField(methodsList, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suiteInstance);

            MethodInfo[] methods = field as MethodInfo[];

            foreach (MethodInfo afterTest in methods)
                afterTest.Invoke(suiteInstance, null);
        }
    }
}
