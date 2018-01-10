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
        private Stopwatch suiteTimer;
        private string name = null;
        private List<string> features = null;
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
        public Guid Id { get; set; }

        /// <summary>
        /// Gets test suite name. If name not specified through TestSuiteAttribute, then return suite class name
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    var attribute = GetType().GetCustomAttribute(typeof(TestSuiteAttribute), true) as TestSuiteAttribute;

                    if (attribute != null)
                    {
                        this.name = attribute.Name;
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
        public List<string> Features
        {
            get
            {
                if (this.features == null)
                {
                    this.features = new List<string>();
                    var attributes = GetType().GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[];

                    foreach (var attribute in attributes)
                    {
                        this.features.Add(attribute.Feature.ToUpper());
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
        public TestSuiteParametersSet CurrentParametersSet { get; set; }

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
        public SuiteOutcome Outcome { get; set; }

        /// <summary>
        /// Gets array of test suite parameters sets (if suite is parameterized)
        /// </summary>
        private TestSuiteParametersSet[] ParametersSets
        {
            get
            {
                if (this.parametersSets == null)
                {
                    var attributes = GetType().GetCustomAttributes(typeof(ParametersSetAttribute), true) as ParametersSetAttribute[];
                    this.parametersSets = new TestSuiteParametersSet[attributes.Length];

                    for (int i = 0; i < attributes.Length; i++)
                    {
                        this.parametersSets[i] = attributes[i].ParametersSet;
                    }
                }

                return this.parametersSets;
            }
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
                Logger.Instance.Error("Exception occured during OnStart event invoke" + Environment.NewLine + ex);
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
                Logger.Instance.Error("Exception occured during OnFinish event invoke" + Environment.NewLine + ex);
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

            try
            {
                OnSkip?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during OnSkip event invoke" + Environment.NewLine + ex);
            }
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

            foreach (var beforeSuite in this.listBeforeSuite)
            {
                beforeSuite.Execute(this);

                if (beforeSuite.Outcome.Result != Result.PASSED)
                {
                    Logger.Instance.Error("Before suite failed, all tests are skipped.");

                    foreach (Test test in this.listTests)
                    {
                        test.Outcome.Result = Result.SKIPPED;
                        test.Outcome.Bugs.Clear();
                    }

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

            foreach (var afterSuite in this.listAfterSuite)
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
        protected List<Test>[] GetTests()
        {
            List<Test>[] testMethods;
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            if (this.ParametersSets.Length == 0)
            {
                this.parametersSets = new TestSuiteParametersSet[1] { null };
            }

            testMethods = new List<Test>[this.ParametersSets.Length];

            for (int i = 0; i < this.ParametersSets.Length; i++)
            {
                testMethods[i] = new List<Test>();

                foreach (MethodInfo method in suiteMethods)
                {
                    if (method.GetCustomAttribute(typeof(TestAttribute), true) == null)
                    {
                        continue;
                    }

                    Test test = new Test(method);
                    test.ParentId = this.Id;
                    test.CheckIfNeedToBeSkipped();

                    string fullTestName = $"{Name} - {method.Name}";
                    string description = $"{test.Description}";

                    if (this.ParametersSets[i] != null)
                    {
                        fullTestName += $" - {ParametersSets[i].Name}";
                        description += $": set[{ParametersSets[i].Name}]";
                    }

                    test.GenerateId();
                    testMethods[i].Add(test);

                    if (!test.IsNeedToBeSkipped)
                    {
                        this.runnableTestsCount++;
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
        /// <param name="attributeType">Type of attribute</param>
        /// <returns>list of MethodInfo with specified attribute</returns>
        private MethodInfo[] GetMethodsListByAttribute(Type attributeType)
        {
            var suitableMethods = new List<MethodInfo>();
            var suiteMethods = GetType().GetRuntimeMethods();

            foreach (var method in suiteMethods)
            {
                var attribute = method.GetCustomAttribute(attributeType, true);

                if (attribute != null)
                {
                    suitableMethods.Add(method);
                }
            }

            return suitableMethods.ToArray();
        }

        /// <summary>
        /// Get list of MethodInfo from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attributeType">Type of attribute</param>
        /// <param name="isBeforeSuite">identify if need to get list of before suites</param>
        /// <returns>list of MethodInfo with specified attribute</returns>
        private TestSuiteMethod[] GetTestSuiteMethodsListByAttribute(Type attributeType, bool isBeforeSuite)
        {
            var suitableMethods = new List<TestSuiteMethod>();
            var suiteMethods = GetType().GetRuntimeMethods();

            foreach (var method in suiteMethods)
            {
                var attribute = method.GetCustomAttribute(attributeType, true);

                if (attribute != null)
                {
                    var suiteMethod = new TestSuiteMethod(method);
                    suiteMethod.ParentId = this.Id;
                    suiteMethod.FullTestName = $"{this.Name} - {method.Name}";
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
