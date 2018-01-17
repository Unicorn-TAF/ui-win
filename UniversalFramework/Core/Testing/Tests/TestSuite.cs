using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Adapter;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        private Stopwatch suiteTimer;
        private string name = null;
        private List<string> features = null;
        private string currentStepBug = string.Empty;
        private bool skipTests = false;

        private Test[] tests;
        private SuiteMethod[] beforeSuites, beforeTests, afterTests, afterSuites;

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
            this.suiteTimer = new Stopwatch();
            this.beforeSuites = GetSuiteMethodsByAttribute(typeof(BeforeSuiteAttribute), SuiteMethodType.BeforeSuite);
            this.beforeTests = GetSuiteMethodsByAttribute(typeof(BeforeTestAttribute), SuiteMethodType.BeforeTest);
            this.afterTests = GetSuiteMethodsByAttribute(typeof(AfterTestAttribute), SuiteMethodType.AfterTest);
            this.afterSuites = GetSuiteMethodsByAttribute(typeof(AfterSuiteAttribute), SuiteMethodType.AfterSuite);
            this.Outcome = new SuiteOutcome();
            this.Outcome.Result = Result.Passed;
            this.tests = GetTests();
        }

        public delegate void UnicornSuiteEvent(TestSuite testSuite);

        public static event UnicornSuiteEvent SuiteStarted;

        public static event UnicornSuiteEvent SuiteFinished;

        public static event UnicornSuiteEvent SuiteSkipped;

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

        public void Execute()
        {
            try
            {
                SuiteStarted?.Invoke(this);
            }
            catch (Exception ex)
            {
                this.Skip("Exception occured during SuiteStarted event invoke" + Environment.NewLine + ex);
                return;
            }

            if (!this.RunSuiteMethods(this.beforeSuites))
            {
                this.Skip("Before Suite failed");
                return;
            }

            Logger.Instance.Info($"==================== TEST SUITE '{this.Name}' ====================");

            this.suiteTimer.Start();

            foreach (Test test in this.tests)
            {
                RunTest(test);
            }

            this.RunSuiteMethods(this.afterSuites);
            this.suiteTimer.Stop();
            this.Outcome.ExecutionTime = this.suiteTimer.Elapsed;

            Logger.Instance.Info($"TEST SUITE {this.Outcome.Result}");

            try
            {
                SuiteFinished?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("Exception occured during SuiteFinished event invoke" + Environment.NewLine + ex);
            }
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        public void Skip(string reason)
        {
            Logger.Instance.Info(reason);

            foreach (Test test in this.tests)
            {
                test.Skip();
            }

            this.Outcome.TotalTests = tests.Length;
            this.Outcome.Result = Result.Skipped;

            try
            {
                SuiteSkipped?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Error("Exception occured during SuiteSkipped event invoke" + Environment.NewLine + e);
            }
        }

        /// <summary>
        /// Run specified Test.
        /// </summary>
        /// <param name="test">test instance</param>
        private void RunTest(Test test)
        {
            this.Outcome.TotalTests++;

            if (this.skipTests)
            {
                test.Skip();
                return;
            }

            if (!this.RunSuiteMethods(this.beforeTests))
            {
                test.Skip();
                return;
            }

            test.Execute(this);

            this.RunAftertests(test.Outcome.Result == Result.Failed);

            if (test.Outcome.Result == Result.Failed)
            {
                this.Outcome.FailedTests++;
                this.Outcome.Result = Result.Failed;

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

                if (suiteMethod.Outcome.Result != Result.Passed)
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
        private void RunAftertests(bool testWasFailed = false)
        {
            foreach (var suiteMethod in this.afterTests)
            {
                var attribute = suiteMethod.TestMethod.GetCustomAttribute(typeof(AfterTestAttribute), true) as AfterTestAttribute;

                if (testWasFailed && !attribute.RunAlways)
                {
                    return;
                }

                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result == Result.Failed)
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
                if (!Helper.IsTestRunnable(method))
                {
                    continue;
                }

                Test test = new Test(method);
                test.MethodType = SuiteMethodType.Test;
                test.ParentId = this.Id;

                string fullTestName = $"{Name} - {method.Name}";
                string description = $"{test.Description}";

                if (GetType().GetCustomAttribute(typeof(ParameterizedAttribute), true) != null)
                {
                    string postfix;

                    if (!this.Metadata.TryGetValue("postfix", out postfix))
                    {
                        postfix = "parameterized";
                    }
                        
                    fullTestName += $" - {postfix}";
                    description += $": set[{postfix}]";
                }

                test.GenerateId();
                testMethods.Add(test);
            }

            return testMethods.ToArray();
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
