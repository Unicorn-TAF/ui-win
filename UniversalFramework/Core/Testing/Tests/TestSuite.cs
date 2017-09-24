using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        private string _name = null;
        /// <summary>
        /// Test suite name. If name not specified through TestSuiteAttribute, then return suite class name
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(TestSuiteAttribute), true);
                    if (attributes.Length != 0)
                        _name = ((TestSuiteAttribute)attributes[0]).Name;
                    else
                        _name = GetType().Name.Split('.').Last();
                }
                return _name;
            }
        }


        private string[] _features = null;
        /// <summary>
        /// Test suite features. Suite could not have any feature
        /// </summary>
        public string[] Features
        {
            get
            {
                if (_features == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(FeatureAttribute), true);
                    if (attributes.Length != 0)
                    {
                        _features = new string[attributes.Length];
                        for (int i = 0; i < attributes.Length; i++)
                            _features[i] = ((FeatureAttribute)attributes[i]).Feature.ToUpper();
                    }
                        
                    else
                        _features = new string[0];
                }
                return _features;
            }
        }


        public Dictionary<string, string> Metadata;

        private static string[] CategoriesToRun;

        private int RunnableTestsCount;

        public string CurrentStepBug = "";

        public SuiteOutcome Outcome;

        Stopwatch SuiteTimer;
        

        private MethodInfo[] ListBeforeSuite;
        private MethodInfo[] ListBeforeTest;
        private MethodInfo[] ListAfterTest;
        private MethodInfo[] ListAfterSuite;
        private List<Test> ListTests;


        public delegate void TestSuiteEvent(TestSuite suite);

        public static event TestSuiteEvent onStart;
        public static event TestSuiteEvent onFinish;


        public TestSuite()
        {
            RunnableTestsCount = 0;
            Metadata = new Dictionary<string, string>();
            if (CategoriesToRun == null)
                CategoriesToRun = new string[0];
            SuiteTimer = new Stopwatch();
            ListBeforeSuite = GetMethodsListByAttribute(typeof(BeforeSuiteAttribute));
            ListBeforeTest = GetMethodsListByAttribute(typeof(BeforeTestAttribute));
            ListAfterTest = GetMethodsListByAttribute(typeof(AfterTestAttribute));
            ListAfterSuite = GetMethodsListByAttribute(typeof(AfterSuiteAttribute));
            ListTests = GetTests();
            Outcome = new SuiteOutcome();
        }


        public static void SetRunCategories(params string[] categoriesToRun)
        {
            CategoriesToRun = categoriesToRun;
        }


        public void Run()
        {
            Logger.Instance.Info($"==================== TEST SUITE '{Name}' ====================");

            onStart?.Invoke(this);

            if (RunnableTestsCount > 0)
            {
                SuiteTimer.Start();

                bool beforeSuitePass = RunBeforeSuite();

                if (beforeSuitePass)
                    foreach (Test test in ListTests)
                        test.Execute(this);

                Outcome.FillWithTestsResults(ListTests);

                if (!RunAfterSuite())
                    Outcome.Result = Result.FAILED;

                SuiteTimer.Stop();
            }
            else
            {
                Skip();
            }

            Outcome.TotalTests = RunnableTestsCount;
            Outcome.ExecutionTime = SuiteTimer.Elapsed;

            onFinish?.Invoke(this);

            Logger.Instance.Info($"Suite {Outcome.Result}");
        }


        public void Skip()
        {
            Logger.Instance.Info("There are no runnable tests");
            RunnableTestsCount = 0;
            Outcome.Result = Result.SKIPPED;
        }


        private bool RunBeforeSuite()
        {
            if (ListBeforeSuite.Length == 0)
                return true;

            try
            {
                foreach (MethodInfo beforeSuite in ListBeforeSuite)
                    beforeSuite.Invoke(this, null);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Before suite failed, all tests are skipped.\n" + ex.ToString());
                Screenshot.TakeScreenshot($"{Name} - BeforeSuite");
            }

            foreach (Test test in ListTests)
                test.Outcome.Result = Result.FAILED;
            //TODO: to clear bugs in outcome (because in fact tests were not run)

            return false;
        }


        private bool RunAfterSuite()
        {
            if (ListAfterSuite.Length == 0)
                return true;

            try
            {
                foreach (MethodInfo afterSuite in ListAfterSuite)
                    afterSuite.Invoke(this, null);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("After suite failed.\n" + ex.ToString());
                Screenshot.TakeScreenshot($"{Name} - AfterSuite");
                return false;
            }
        }


        #region Helpers

        /// <summary>
        /// Get list of MethodInfos from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attribute">Type of attribute</param>
        /// <returns>list of MethodInfos with specified attribute</returns>
        private MethodInfo[] GetMethodsListByAttribute(Type attribute)
        {
            List<MethodInfo> suitableMethods = new List<MethodInfo>();
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            foreach (MethodInfo method in suiteMethods)
            {
                object[] attributes = method.GetCustomAttributes(attribute, true);

                if (attributes.Length != 0)
                    suitableMethods.Add(method);
            }
            return suitableMethods.ToArray();
        }


        /// <summary>
        /// Get list of Tests from suite instance based on [Test] Attribute presence. 
        /// Determine if test should be skipped and update runnable tests count for the suite. 
        /// </summary>
        /// <returns>list of Tests</returns>
        private List<Test> GetTests()
        {
            List<Test> testMethods = new List<Test>();
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            foreach (MethodInfo method in suiteMethods)
            {
                object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), true);

                if (attributes.Length != 0)
                {
                    Test test = new Test(method);
                    test.CheckIfNeedToBeSkipped(CategoriesToRun);
                    test.FullTestName = $"{Name} - {method.Name}";
                    testMethods.Add(test);

                    if (!test.IsNeedToBeSkipped)
                        RunnableTestsCount++;
                }
            }
            return testMethods;
        }
        
        #endregion
    }
}
