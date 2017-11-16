using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class Test : TestSuiteMethodBase
    {

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
            set
            {
                _description = value;
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
                            _categories[i] = ((CategoryAttribute)attributes[i]).Category.ToUpper().Trim();
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


        // Events section

        public delegate void TestEvent(Test test);

        public static event TestEvent onStart;
        public static event TestEvent onFinish;
        public static event TestEvent onPass;
        public static event TestEvent onFail;
        public static event TestEvent onSkip;



        /// <summary>
        /// Object describing single test, which is part of some TestSuite.
        /// Contains list of events related to different Test states (started, finished, skipped, passed, failed)
        /// Contains methods to execute the test and check if test should be skipped
        /// </summary>
        /// <param name="testMethod">MethodInfo instance which represents test method</param>
        public Test(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome();
            IsNeedToBeSkipped = false;
        }



        /// <summary>
        /// Execute current test and fill TestOutcome
        /// Before the test List of BeforeTests is executed
        /// After the test List of AfterTests is executed
        /// </summary>
        /// <param name="suiteInstance">test suite instance to run in</param>
        public override void Execute(TestSuite suiteInstance)
        {
            if (IsNeedToBeSkipped)
            {
                Skip();
                return;
            }

            CurrentOutput = new StringBuilder();
            onStart?.Invoke(this);

            Logger.Instance.Info($"========== TEST '{Description}' ==========");

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

            Logger.Instance.Info($"Test {Outcome.Result}");

            onFinish?.Invoke(this);
        }


        /// <summary>
        /// Skip test and invoke onSkip event
        /// </summary>
        private void Skip()
        {
            Outcome.Result = Result.SKIPPED;
            onSkip?.Invoke(this);
            Logger.Instance.Info($"Test '{Description}' {Outcome.Result}");
        }


        /// <summary>
        /// Fail test, fill TestOutcome exception and bugs and invoke onFail event.
        /// If test failed not by existing bug it is marked as 'To investigate'
        /// </summary>
        /// <param name="ex">Exception caught on test execution</param>
        /// <param name="bugs">string of bugs test failed on current step.</param>
        protected override void Fail(Exception ex, string bugs)
        {
            
            Logger.Instance.Error(ex.ToString());

            if (!string.IsNullOrEmpty(bugs))
                Outcome.Bugs = bugs.Split(',');
            else
                Outcome.Bugs = new string[] { "?" };

            Outcome.Exception = ex;
            Outcome.Result = Result.FAILED;
            onFail?.Invoke(this);
        }


        /// <summary>
        /// Check if test should be skipped. 
        /// Test is skipped if it does not contain at least one of specified categories
        /// Result of the check is stored in IsNeedToBeSkipped field
        /// </summary>
        /// <param name="categories">list of expected categories to run</param>
        public void CheckIfNeedToBeSkipped(params string[] categories)
        {
            object[] attributes = TestMethod.GetCustomAttributes(typeof(SkipAttribute), true);

            IsNeedToBeSkipped = attributes.Length != 0;

            if (categories.Length > 0)
                IsNeedToBeSkipped |= Categories.Intersect(categories).Count() != categories.Count();
        }


        /// <summary>
        /// Execute list of MethodInfos from TestSuite instance based on Field nameof TestSuite class.
        /// Used to run BeforeTests and AfterTests
        /// </summary>
        /// <param name="suiteInstance">instance of TestSuite</param>
        /// <param name="methodsList">name of Field contained array of MethodInfos</param>
        private void ExecuteMethods(TestSuite suiteInstance, string methodsList)
        {
            object field = typeof(TestSuite)
                .GetField(methodsList, BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(suiteInstance);

            MethodInfo[] methods = field as MethodInfo[];

            foreach (MethodInfo method in methods)
                method.Invoke(suiteInstance, null);
        }
    }
}
