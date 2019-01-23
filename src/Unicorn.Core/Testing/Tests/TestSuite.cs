using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unicorn.Core.Engine;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        private Stopwatch suiteTimer;
        private string name = null;
        private List<string> features = null;
        private bool skipTests = false;

        private readonly Test[] tests;
        private readonly SuiteMethod[] beforeSuites;
        private readonly SuiteMethod[] beforeTests;
        private readonly SuiteMethod[] afterTests;
        private readonly SuiteMethod[] afterSuites;

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
            this.Metadata = new Dictionary<string, string>();

            foreach (var attribute in GetType().GetCustomAttributes(typeof(MetadataAttribute), true) as MetadataAttribute[])
            {
                this.Metadata.Add(attribute.Key, attribute.Value);
            }

            this.beforeSuites = GetSuiteMethodsByAttribute(typeof(BeforeSuiteAttribute), SuiteMethodType.BeforeSuite);
            this.beforeTests = GetSuiteMethodsByAttribute(typeof(BeforeTestAttribute), SuiteMethodType.BeforeTest);
            this.afterTests = GetSuiteMethodsByAttribute(typeof(AfterTestAttribute), SuiteMethodType.AfterTest);
            this.afterSuites = GetSuiteMethodsByAttribute(typeof(AfterSuiteAttribute), SuiteMethodType.AfterSuite);
            this.Outcome = new SuiteOutcome();
            this.Outcome.Result = Status.Passed;
            this.tests = GetTests();
        }

        public delegate void UnicornSuiteEvent(TestSuite testSuite);

        public static event UnicornSuiteEvent OnSuiteStart;
        public static event UnicornSuiteEvent OnSuiteFinish;
        public static event UnicornSuiteEvent OnSuitePass;
        public static event UnicornSuiteEvent OnSuiteFail;
        public static event UnicornSuiteEvent OnSuiteSkip;

        // Gets or sets Unique suite Guid
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets test suite name. If name not specified through TestSuiteAttribute, then return suite class name
        /// </summary>
        public string Name
        {
            get
            {
                if (this.name == null)
                {
                    var attribute = GetType().GetCustomAttribute(typeof(TestSuiteAttribute), true) as TestSuiteAttribute;

                    this.name = attribute != null ? attribute.Name : GetType().Name.Split('.').Last();
                }

                return this.name;
            }

            set
            {
                this.name = value;
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
                    var attributes = GetType().GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[];
                    this.features = (from attribute in attributes select attribute.Feature.ToUpper()).ToList();
                }

                return this.features;
            }
        }

        /// <summary>
        /// Gets TestSuite metadata dictionary, can contain only string values
        /// </summary>
        public Dictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets or sets current executing step bug, used in case of TestSteps feature usage
        /// </summary>
        public string CurrentStepBug { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets Suite outcome, contain all information on suite run and results
        /// </summary>
        public SuiteOutcome Outcome { get; protected set; }

        public void Execute()
        {
            Logger.Instance.Log(LogLevel.Info, $"==================== TEST SUITE '{this.Name}' ====================");

            try
            {
                OnSuiteStart?.Invoke(this);
                this.RunSuite();
            }
            catch (Exception ex)
            {
                this.Skip("Exception occured during OnSuiteStart event invoke" + Environment.NewLine + ex);
            }
            finally
            {
                try
                {
                    OnSuiteFinish?.Invoke(this);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteFinish event invoke" + Environment.NewLine + ex);
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"TEST SUITE {this.Outcome.Result}");
        }

        private void RunSuite()
        {
            this.suiteTimer = Stopwatch.StartNew();

            if (this.RunSuiteMethods(this.beforeSuites))
            {
                foreach (Test test in this.tests)
                {
                    Thread testThread = new Thread(() => this.RunTest(test));
                    testThread.Start();

                    if (!testThread.Join(Configuration.TestTimeout))
                    {
                        testThread.Abort();
                        test.Fail(new TimeoutException(string.Format("Test timeout ({0:F1} minutes) reached", Configuration.TestTimeout.TotalMinutes)), string.Empty);
                    }

                    this.Outcome.TestsOutcomes.Add(test.Outcome);
                }
            }
            else
            {
                this.Skip("Before Suite failed");
            }

            this.RunSuiteMethods(this.afterSuites);

            this.suiteTimer.Stop();
            this.Outcome.ExecutionTime = this.suiteTimer.Elapsed;
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        private void Skip(string reason)
        {
            Logger.Instance.Log(LogLevel.Info, reason);

            foreach (Test test in this.tests)
            {
                test.Skip(reason);
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Description}' {Outcome.Result}");
                this.Outcome.TestsOutcomes.Add(test.Outcome);
            }

            this.Outcome.Result = Status.Skipped;

            try
            {
                OnSuiteSkip?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Error, "Exception occured during OnSuiteSkip event invoke" + Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Run specified Test.
        /// </summary>
        /// <param name="test">test instance</param>
        private void RunTest(Test test)
        {
            if (!test.IsRunnable)
            {
                test.Outcome.Result = Status.NotExecuted;
                return;
            }

            if (this.skipTests)
            {
                test.Skip("Previuos test cleanup failed");
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Description}' {Outcome.Result}");
                return;
            }

            if (!this.RunSuiteMethods(this.beforeTests))
            {
                test.Skip(string.Empty);
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Description}' {Outcome.Result}");
                return;
            }

            test.Execute(this);

            this.RunAftertests(test.Outcome.Result == Status.Failed);

            if (test.Outcome.Result == Status.Failed)
            {
                this.Outcome.Result = Status.Failed;

                foreach (var bug in test.Outcome.Bugs)
                {
                    this.Outcome.Bugs.Add(bug);
                }
            }
        }

        #region Helpers

        /// <summary>
        /// Run SuiteMethods
        /// </summary>
        /// <param name="suiteMethods">array of suite methods to run</param>
        /// <returns>true if after suites run successfully; fail if at least one after suite failed</returns>
        private bool RunSuiteMethods(SuiteMethod[] suiteMethods)
        {
            foreach (var suiteMethod in suiteMethods)
            {
                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result != Status.Passed)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Run SuiteMethods
        /// </summary>
        /// <param name="testWasFailed">array of suite methods to run</param>
        private void RunAftertests(bool testWasFailed)
        {
            foreach (var suiteMethod in this.afterTests)
            {
                var attribute = suiteMethod.TestMethod.GetCustomAttribute(typeof(AfterTestAttribute), true) as AfterTestAttribute;

                if (testWasFailed && !attribute.RunAlways)
                {
                    return;
                }

                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result == Status.Failed)
                {
                    skipTests = attribute.SkipTestsOnFail && Configuration.ParallelBy != Parallelization.Test;
                }
            }
        }

        /// <summary>
        /// Get list of Tests from suite instance based on [Test] Attribute presence. 
        /// Determine if test should be skipped and update runnable tests count for the suite. 
        /// </summary>
        /// <returns>list of Tests</returns>
        private Test[] GetTests()
        {
            List<Test> testMethods = new List<Test>();

            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(typeof(TestAttribute), true) != null);

            foreach (MethodInfo method in suiteMethods)
            {
                if (AdapterUtilities.IsTestParameterized(method))
                {
                    var attribute = method.GetCustomAttribute(typeof(TestDataAttribute), true) as TestDataAttribute;
                    foreach (DataSet dataSet in AdapterUtilities.GetTestData(attribute.Method, this))
                    {
                        Test test = GenerateTest(method, dataSet);
                        testMethods.Add(test);
                    }
                }
                else
                {
                    Test test = GenerateTest(method, null);
                    testMethods.Add(test);
                }
            }

            return testMethods.ToArray();
        }

        /// <summary>
        /// Generate instance of <see cref="Test"/> and fill with all data
        /// </summary>
        /// <param name="method">MethodInfo instance which represents test method</param>
        /// <param name="dataSet">DataSet to populate test method parameters; null if method does not have parameters</param>
        /// <returns>Test instance</returns>
        private Test GenerateTest(MethodInfo method, DataSet dataSet)
        {
            Test test;

            if (dataSet == null)
            {
                test = new Test(method);
            }
            else
            {
                test = new Test(method, dataSet);
            }
             
            test.MethodType = SuiteMethodType.Test;
            test.ParentId = this.Id;
            test.IsRunnable = AdapterUtilities.IsTestRunnable(method);

            string fullTestName = $"{Name} - {method.Name}";
            string description = $"{test.Description}";

            if (dataSet != null)
            {
                fullTestName += $" - {dataSet.Name}";
                description += $" [{dataSet.Name}]";
            }

            test.FullName = fullTestName;
            test.Description = description;
            test.GenerateId();

            return test;
        }

        /// <summary>
        /// Get list of MethodInfo from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attributeType">Type of attribute</param>
        /// <param name="type">type of suite method (<see cref="SuiteMethodType"/>)</param>
        /// <returns>list of MethodInfo with specified attribute</returns>
        private SuiteMethod[] GetSuiteMethodsByAttribute(Type attributeType, SuiteMethodType type)
        {
            var suitableMethods = new List<SuiteMethod>();
            var suiteMethods = GetType().GetRuntimeMethods();

            foreach (var method in suiteMethods)
            {
                var attribute = method.GetCustomAttribute(attributeType, true);

                if (attribute != null)
                {
                    var suiteMethod = new SuiteMethod(method);
                    suiteMethod.ParentId = this.Id;
                    suiteMethod.FullName = $"{this.Name} - {method.Name}";
                    suiteMethod.GenerateId();
                    suiteMethod.MethodType = type;
                    suitableMethods.Add(suiteMethod);
                }
            }

            return suitableMethods.ToArray();
        }

        #endregion
    }
}
