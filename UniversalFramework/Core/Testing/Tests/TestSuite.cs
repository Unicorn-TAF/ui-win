using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        // Unique suite Guid
        public Guid Id;

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


        private TestSuiteParametersSet[] _parametersSets = null;

        /// <summary>
        /// Array of test suite parameters sets (if suite is parameterized)
        /// </summary>
        private TestSuiteParametersSet[] ParametersSets
        {
            get
            {
                if (_parametersSets == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(ParametersSetAttribute), true);
                    if (attributes.Length != 0)
                    {
                        _parametersSets = new TestSuiteParametersSet[attributes.Length];
                        for (int i = 0; i < attributes.Length; i++)
                            _parametersSets[i] = ((ParametersSetAttribute)attributes[i]).ParametersSet;
                    }

                    else
                        _parametersSets = new TestSuiteParametersSet[0];
                }
                return _parametersSets;
            }
        }


        /// <summary>
        /// Current parameters set for the suite iteration (if suite is parameterized)
        /// </summary>
        public TestSuiteParametersSet CurrentParametersSet;


        public Dictionary<string, string> Metadata;

        private static string[] CategoriesToRun;

        private int RunnableTestsCount;

        public string CurrentStepBug = "";

        public SuiteOutcome Outcome;

        Stopwatch SuiteTimer;
        

        private TestSuiteMethod[] ListBeforeSuite;
        private MethodInfo[] ListBeforeTest;
        private MethodInfo[] ListAfterTest;
        private TestSuiteMethod[] ListAfterSuite;
        protected List<Test>[] ListTestsAll;
        protected List<Test> ListTests;


        public delegate void TestSuiteEvent(TestSuite suite);


        public static event TestSuiteEvent onStart;
        public static event TestSuiteEvent onFinish;


        /// <summary>
        /// Object Describing TestSuite.
        /// Contains List of tests and Lists of Before and AfterSuites.
        /// Contains list of events related to different Suite states (started, finished, skipped)
        /// On Initialize the list of Tests, BeforeTests, AfterTests, BeforeSuites and AfterSuites is retrieved from the instance.
        /// For each test is performed check for skip
        /// </summary>
        public TestSuite()
        {
            Id = Guid.NewGuid();
            RunnableTestsCount = 0;
            Metadata = new Dictionary<string, string>();
            if (CategoriesToRun == null)
                CategoriesToRun = new string[0];
            SuiteTimer = new Stopwatch();
            ListBeforeSuite = GetTestSuiteMethodsListByAttribute(typeof(BeforeSuiteAttribute), true);
            ListBeforeTest = GetMethodsListByAttribute(typeof(BeforeTestAttribute));
            ListAfterTest = GetMethodsListByAttribute(typeof(AfterTestAttribute));
            ListAfterSuite = GetTestSuiteMethodsListByAttribute(typeof(AfterSuiteAttribute), false);
            Outcome = new SuiteOutcome();
            Outcome.Result = Result.PASSED;
            ListTestsAll = GetTests();
        }


        /// <summary>
        /// Set array of tests categories needed to be run.
        /// All categories are converted in upper case.
        /// Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetRunCategories(params string[] categoriesToRun)
        {
            CategoriesToRun = categoriesToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();
        }


        /// <summary>
        /// Run Test suite and all Before and After suites invoking corresponding suite events.
        /// If there are no runna ble tests, the suite is skipped.
        /// If BeforeSuite is failed, the suite is skipped.
        /// If All tests are passed, but AfterSuite is failed, the suite is failed.
        /// After run SuiteOutcome is filled with all info.
        /// </summary>
        public void Run()
        {
            Logger.Instance.Info($"==================== TEST SUITE '{Name}' ====================");

            onStart?.Invoke(this);

            if (RunnableTestsCount > 0)
            {
                SuiteTimer.Start();

                ExecuteWholeSuite();

                SuiteTimer.Stop();
            }
            else
            {
                Skip();
            }

            Outcome.TotalTests = RunnableTestsCount;
            Outcome.ExecutionTime = SuiteTimer.Elapsed;

            Logger.Instance.Info($"SUITE {Outcome.Result}");

            onFinish?.Invoke(this);
        }


        private void ExecuteWholeSuite()
        {
            for (int i = 0; i < ListTestsAll.Length; i++)
            {
                if(ParametersSets.Length > 0)
                    CurrentParametersSet = ParametersSets[i];

                ListTests = ListTestsAll[i];

                if (RunBeforeSuite())
                    foreach (Test test in ListTests)
                        test.Execute(this);

                Outcome.FillWithTestsResults(ListTests);

                if (!RunAfterSuite())
                    Outcome.Result = Result.FAILED;
            }
        }


        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        public void Skip()
        {
            Logger.Instance.Info("There are no runnable tests");
            RunnableTestsCount = 0;
            Outcome.Result = Result.SKIPPED;
        }


        /// <summary>
        /// Run BeforeSuites
        /// </summary>
        /// <returns></returns>
        protected bool RunBeforeSuite()
        {
            if (ListBeforeSuite.Length == 0)
                return true;

            foreach (TestSuiteMethod beforeSuite in ListBeforeSuite)
            {
                beforeSuite.Execute(this);
                if (beforeSuite.Outcome.Result != Result.PASSED)
                {
                    Logger.Instance.Error("Before suite failed, all tests are skipped.");

                    foreach (Test test in ListTests)
                        test.Outcome.Result = Result.FAILED;
                    //TODO: to clear bugs in outcome (because in fact tests were not run)

                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Run AfterSuites
        /// </summary>
        /// <returns></returns>
        protected bool RunAfterSuite()
        {
            if (ListAfterSuite.Length == 0)
                return true;

            foreach (TestSuiteMethod afterSuite in ListAfterSuite)
            {
                afterSuite.Execute(this);
                if (afterSuite.Outcome.Result != Result.PASSED)
                {
                    Logger.Instance.Error("After suite failed.");
                    return false;
                }
            }

            return true;
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
        /// Get list of MethodInfos from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attribute">Type of attribute</param>
        /// <returns>list of MethodInfos with specified attribute</returns>
        private TestSuiteMethod[] GetTestSuiteMethodsListByAttribute(Type attribute, bool isBeforeSuite)
        {
            List<TestSuiteMethod> suitableMethods = new List<TestSuiteMethod>();
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            foreach (MethodInfo method in suiteMethods)
            {
                object[] attributes = method.GetCustomAttributes(attribute, true);

                if (attributes.Length != 0)
                {
                    TestSuiteMethod suiteMethod = new TestSuiteMethod(method);
                    suiteMethod.ParentId = Id;
                    suiteMethod.FullTestName = $"{Name} - {method.Name}";
                    suiteMethod.GenerateId();
                    suiteMethod.IsBeforeSuite = isBeforeSuite;
                    suitableMethods.Add(suiteMethod);
                }
            }
            return suitableMethods.ToArray();
        }


        /// <summary>
        /// Get list of Tests from suite instance based on [Test] Attribute presence. 
        /// Determine if test should be skipped and update runnable tests count for the suite. 
        /// </summary>
        /// <returns>list of Tests</returns>
        protected virtual List<Test>[] GetTests()
        {
            List<Test>[] testMethods;
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            if (ParametersSets.Length == 0)
            {
                testMethods = new List<Test>[1];
                testMethods[0] = new List<Test>();

                foreach (MethodInfo method in suiteMethods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), true);

                    if (attributes.Length != 0)
                    {
                        Test test = new Test(method);
                        test.ParentId = Id;
                        test.CheckIfNeedToBeSkipped(CategoriesToRun);
                        test.FullTestName = $"{Name} - {method.Name}";
                        test.GenerateId();
                        testMethods[0].Add(test);

                        if (!test.IsNeedToBeSkipped)
                            RunnableTestsCount++;
                    }
                }
            }
            else
            {
                testMethods = new List<Test>[ParametersSets.Length];

                for (int i = 0; i < ParametersSets.Length; i++)
                {
                    testMethods[i] = new List<Test>();

                    foreach (MethodInfo method in suiteMethods)
                    {
                        object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), true);

                        if (attributes.Length != 0)
                        {
                            Test test = new Test(method);
                            test.ParentId = Id;
                            test.CheckIfNeedToBeSkipped(CategoriesToRun);
                            test.FullTestName = $"{Name} - {method.Name} - {ParametersSets[i].SetName}";
                            test.Description = $"{test.Description}: set[{ParametersSets[i].SetName}]";
                            test.GenerateId();
                            testMethods[i].Add(test);

                            if (!test.IsNeedToBeSkipped)
                                RunnableTestsCount++;
                        }
                    }
                }
            }
            return testMethods;
        }


        #endregion
    }
}
