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
        private static string[] categoriesToRun;
        private Stopwatch suiteTimer;
        private string name = null;
        private string[] features = null;
        private TestSuiteParametersSet[] parametersSets = null;
        private int runnableTestsCount;
        private string currentStepBug = string.Empty;

        private List<Test>[] listTestsAll;
        private List<Test> listTests;
        private TestSuiteMethod[] listBeforeSuite;
        private MethodInfo[] listBeforeTest;
        private MethodInfo[] listAfterTest;
        private TestSuiteMethod[] listAfterSuite;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.
        /// Contains List of tests and Lists of Before and AfterSuites.
        /// Contains list of events related to different Suite states (started, finished, skipped)
        /// On Initialize the list of Tests, BeforeTests, AfterTests, BeforeSuites and AfterSuites is retrieved from the instance.
        /// For each test is performed check for skip
        /// </summary>
        public TestSuite()
        {
            this.Id = Guid.NewGuid();
            this.runnableTestsCount = 0;
            this.Metadata = new Dictionary<string, string>();

            if (categoriesToRun == null)
            {
                categoriesToRun = new string[0];
            }

            this.suiteTimer = new Stopwatch();
            this.listBeforeSuite = GetTestSuiteMethodsListByAttribute(typeof(BeforeSuiteAttribute), true);
            this.listBeforeTest = GetMethodsListByAttribute(typeof(BeforeTestAttribute));
            this.listAfterTest = GetMethodsListByAttribute(typeof(AfterTestAttribute));
            this.listAfterSuite = GetTestSuiteMethodsListByAttribute(typeof(AfterSuiteAttribute), false);
            this.Outcome = new SuiteOutcome();
            this.Outcome.Result = Result.PASSED;
            this.listTestsAll = GetTests();
        }

        public delegate void TestSuiteEvent(TestSuite suite);

        /// <summary>
        /// Event raised on TestSuite start
        /// </summary>
        public static event TestSuiteEvent OnStart;

        /// <summary>
        /// Event raised on TestSuite finish
        /// </summary>
        public static event TestSuiteEvent OnFinish;

        /// <summary>
        /// Event raised on TestSuite skip
        /// </summary>
        public static event TestSuiteEvent OnSkip;

        // Gets or sets Unique suite Guid
        public Guid Id
        {
            get;

            set;
        }

        /// <summary>
        /// Gets test suite name. If name not specified through TestSuiteAttribute, then return suite class name
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(TestSuiteAttribute), true);
                    if (attributes.Length != 0)
                    {
                        this.name = ((TestSuiteAttribute)attributes[0]).Name;
                    }
                    else
                    {
                        this.name = GetType().Name.Split('.').Last();
                    }
                }

                return this.name;
            }
        }

        /// <summary>
        /// Gets test suite features. Suite could not have any feature
        /// </summary>
        public string[] Features
        {
            get
            {
                if (this.features == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(FeatureAttribute), true);

                    if (attributes.Length != 0)
                    {
                        this.features = new string[attributes.Length];

                        for (int i = 0; i < attributes.Length; i++)
                        {
                            this.features[i] = ((FeatureAttribute)attributes[i]).Feature.ToUpper();
                        }
                    }
                    else
                    {
                        this.features = new string[0];
                    }
                }

                return this.features;
            }
        }

        /// <summary>
        /// Gets TestSuite metadata dictionary, can contain only string values
        /// </summary>
        public Dictionary<string, string> Metadata
        {
            get;
        }

        /// <summary>
        /// Gets or sets Current parameters set for the suite iteration (if suite is parameterized)
        /// </summary>
        public TestSuiteParametersSet CurrentParametersSet
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets current executing step bug, used in case of TestSteps feature usage
        /// </summary>
        public string CurrentStepBug
        {
            get
            {
                return this.currentStepBug;
            }

            set
            {
                this.currentStepBug = value;
            }
        }

        /// <summary>
        /// Gets or sets Suite outcome, contain all information on suite run and results
        /// </summary>
        public SuiteOutcome Outcome
        {
            get;

            set;
        }

        /// <summary>
        /// Gets array of test suite parameters sets (if suite is parameterized)
        /// </summary>
        private TestSuiteParametersSet[] ParametersSets
        {
            get
            {
                if (this.parametersSets == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(ParametersSetAttribute), true);

                    if (attributes.Length != 0)
                    {
                        this.parametersSets = new TestSuiteParametersSet[attributes.Length];

                        for (int i = 0; i < attributes.Length; i++)
                        {
                            this.parametersSets[i] = ((ParametersSetAttribute)attributes[i]).ParametersSet;
                        }
                    }
                    else
                    {
                        this.parametersSets = new TestSuiteParametersSet[0];
                    }
                }

                return this.parametersSets;
            }
        }

        /// <summary>
        /// Set array of tests categories needed to be run.
        /// All categories are converted in upper case.
        /// Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRunArray">array of categories</param>
        public static void SetRunCategories(params string[] categoriesToRunArray)
        {
            categoriesToRun = categoriesToRunArray
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToArray();
        }

        /// <summary>
        /// Run Test suite and all Before and After suites invoking corresponding suite events.
        /// If there are no runnable tests, the suite is skipped.
        /// If BeforeSuite is failed, the suite is skipped.
        /// If All tests are passed, but AfterSuite is failed, the suite is failed.
        /// After run SuiteOutcome is filled with all info.
        /// </summary>
        public void Run()
        {
            if (this.runnableTestsCount == 0)
            {
                Skip("There are no runnable tests");
                return;
            }

            try
            {
                OnStart?.Invoke(this);
            }
            catch (Exception ex)
            {
                Skip("Error running test suite");
                Logger.Instance.Error("Exception occured during onStart event invoke" + Environment.NewLine + ex);
                return;
            }

            Logger.Instance.Info($"==================== TEST SUITE '{Name}' ====================");

            this.suiteTimer.Start();

            ExecuteWholeSuite();

            this.suiteTimer.Stop();

            this.Outcome.TotalTests = this.runnableTestsCount;

            this.Outcome.ExecutionTime = this.suiteTimer.Elapsed;
            Logger.Instance.Info($"TEST SUITE {this.Outcome.Result}");

            try
            {
                OnFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during onFinish event invoke" + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        public void Skip(string reason)
        {
            Logger.Instance.Info(reason);
            this.runnableTestsCount = 0;
            this.Outcome.Result = Result.SKIPPED;
            OnSkip?.Invoke(this);
        }

        /// <summary>
        /// Run BeforeSuites
        /// </summary>
        /// <returns>true - if before suites passed; false - if at least one Before suite is failed</returns>
        protected bool RunBeforeSuite()
        {
            if (this.listBeforeSuite.Length == 0)
            {
                return true;
            }

            foreach (TestSuiteMethod beforeSuite in this.listBeforeSuite)
            {
                beforeSuite.Execute(this);
                if (beforeSuite.Outcome.Result != Result.PASSED)
                {
                    Logger.Instance.Error("Before suite failed, all tests are skipped.");

                    foreach (Test test in this.listTests)
                    {
                        test.Outcome.Result = Result.FAILED;
                    }

                    // TODO: to clear bugs in outcome (because in fact tests were not run)
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Run AfterSuites
        /// </summary>
        /// <returns>true if after suites run successfully; fail if at least one after suite failed</returns>
        protected bool RunAfterSuite()
        {
            if (this.listAfterSuite.Length == 0)
            {
                return true;
            }

            foreach (TestSuiteMethod afterSuite in this.listAfterSuite)
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

        /// <summary>
        /// Get list of Tests from suite instance based on [Test] Attribute presence. 
        /// Determine if test should be skipped and update runnable tests count for the suite. 
        /// </summary>
        /// <returns>list of Tests</returns>
        protected virtual List<Test>[] GetTests()
        {
            List<Test>[] testMethods;
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            if (this.ParametersSets.Length == 0)
            {
                testMethods = new List<Test>[1];
                testMethods[0] = new List<Test>();

                foreach (MethodInfo method in suiteMethods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), true);

                    if (attributes.Length != 0)
                    {
                        Test test = new Test(method);
                        test.ParentId = this.Id;
                        test.CheckIfNeedToBeSkipped(categoriesToRun);
                        test.FullTestName = $"{Name} - {method.Name}";
                        test.GenerateId();
                        testMethods[0].Add(test);

                        if (!test.IsNeedToBeSkipped)
                        {
                            this.runnableTestsCount++;
                        }
                    }
                }
            }
            else
            {
                testMethods = new List<Test>[this.ParametersSets.Length];

                for (int i = 0; i < this.ParametersSets.Length; i++)
                {
                    testMethods[i] = new List<Test>();

                    foreach (MethodInfo method in suiteMethods)
                    {
                        object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), true);

                        if (attributes.Length != 0)
                        {
                            Test test = new Test(method);
                            test.ParentId = this.Id;
                            test.CheckIfNeedToBeSkipped(categoriesToRun);
                            test.FullTestName = $"{Name} - {method.Name} - {ParametersSets[i].Name}";
                            test.Description = $"{test.Description}: set[{ParametersSets[i].Name}]";
                            test.GenerateId();
                            testMethods[i].Add(test);

                            if (!test.IsNeedToBeSkipped)
                            {
                                this.runnableTestsCount++;
                            }
                        }
                    }
                }
            }

            return testMethods;
        }

        #region Helpers

        private void ExecuteWholeSuite()
        {
            for (int i = 0; i < this.listTestsAll.Length; i++)
            {
                if (this.ParametersSets.Length > 0)
                {
                    this.CurrentParametersSet = this.ParametersSets[i];
                }

                this.listTests = this.listTestsAll[i];

                if (this.RunBeforeSuite())
                {
                    foreach (Test test in this.listTests)
                    {
                        test.Execute(this);
                    }
                }

                this.Outcome.FillWithTestsResults(this.listTests);

                if (!this.RunAfterSuite())
                {
                    this.Outcome.Result = Result.FAILED;
                }
            }
        }

        /// <summary>
        /// Get list of MethodInfo from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attribute">Type of attribute</param>
        /// <returns>list of MethodInfo with specified attribute</returns>
        private MethodInfo[] GetMethodsListByAttribute(Type attribute)
        {
            List<MethodInfo> suitableMethods = new List<MethodInfo>();
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            foreach (MethodInfo method in suiteMethods)
            {
                object[] attributes = method.GetCustomAttributes(attribute, true);

                if (attributes.Length != 0)
                {
                    suitableMethods.Add(method);
                }
            }

            return suitableMethods.ToArray();
        }

        /// <summary>
        /// Get list of MethodInfo from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attribute">Type of attribute</param>
        /// <param name="isBeforeSuite">identify if need to get list of before suites</param>
        /// <returns>list of MethodInfo with specified attribute</returns>
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
                    suiteMethod.ParentId = this.Id;
                    suiteMethod.FullTestName = $"{Name} - {method.Name}";
                    suiteMethod.GenerateId();
                    suiteMethod.IsBeforeSuite = isBeforeSuite;
                    suitableMethods.Add(suiteMethod);
                }
            }

            return suitableMethods.ToArray();
        }

        #endregion
    }
}
