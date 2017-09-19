using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
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
                            _categories[i] = ((CategoryAttribute)attributes[0]).Category;
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


        public void CheckIfNeedToBeSkipped(params string[] categories)
        {
            object[] attributes = TestMethod.GetCustomAttributes(typeof(SkipAttribute), true);

            IsNeedToBeSkipped = attributes.Length != 0;

            if (categories.Length > 0)
                IsNeedToBeSkipped |= Categories.Intersect(categories).Count() != categories.Count();
        }


        public TestOutcome Outcome;

        public Stopwatch TestTimer;

        private MethodInfo TestMethod;
        

        public Test(MethodInfo testMethod)
        {
            TestMethod = testMethod;
            Outcome = new TestOutcome();
            IsNeedToBeSkipped = false;
        }



        public void Execute(TestSuite suiteInstance)
        {
            Logger.Instance.Info($"========== TEST '{Description}' ==========");
            Reporter.Instance.ReportTestStart(this);
            TestTimer = new Stopwatch();
            
            if (IsNeedToBeSkipped)
            {
                Outcome.Result = Result.SKIPPED;
            }
            else
            {
                TestTimer.Start();
                try
                {
                    ExecuteMethods(suiteInstance, "ListBeforeTest");
                    TestMethod.Invoke(suiteInstance, null);
                    ExecuteMethods(suiteInstance, "ListAfterTest");
                    Outcome.Result = Result.PASSED;
                }
                catch (Exception ex)
                {
                    Logger.Instance.Error(ex.InnerException.ToString());

                    string screenshotFile = $"{suiteInstance.Name} - {TestMethod.Name}";
                    Screenshot.TakeScreenshot(screenshotFile);
                    Outcome.Screenshot = screenshotFile + ".Jpeg";

                    if (!string.IsNullOrEmpty(suiteInstance.CurrentStepBug))
                        Outcome.Bugs = suiteInstance.CurrentStepBug.Split(',');

                    Outcome.Exception = ex.InnerException;
                    Outcome.Result = Result.FAILED;
                }

                TestTimer.Stop();
            }
            
            Outcome.ExecutionTime = TestTimer.Elapsed;

            Reporter.Instance.ReportTestFinish(this);
            Logger.Instance.Info($"Test {Outcome.Result}");
        }


        public void Skip()
        {
            Outcome.Result = Result.SKIPPED;
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
