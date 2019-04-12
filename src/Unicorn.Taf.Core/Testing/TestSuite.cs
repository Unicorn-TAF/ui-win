using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    public class TestSuite
    {
        private readonly Test[] tests;
        private readonly SuiteMethod[] beforeSuites;
        private readonly SuiteMethod[] beforeTests;
        private readonly SuiteMethod[] afterTests;
        private readonly SuiteMethod[] afterSuites;

        private string name = null;
        private List<string> tags = null;
        private bool skipTests = false;

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
                    var attribute = GetType().GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute;

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
        public List<string> Tags
        {
            get
            {
                if (this.tags == null)
                {
                    var attributes = GetType().GetCustomAttributes(typeof(TagAttribute), true) as TagAttribute[];
                    this.tags = (from attribute in attributes select attribute.Tag.ToUpper()).ToList();
                }

                return this.tags;
            }
        }

        /// <summary>
        /// Gets TestSuite metadata dictionary, can contain only string values
        /// </summary>
        public Dictionary<string, string> Metadata { get; }

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
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteFinish event invoke" + Environment.NewLine + ex);
                }
            }

            Logger.Instance.Log(LogLevel.Info, $"TEST SUITE {this.Outcome.Result}");
        }

        private void RunSuite()
        {
            var suiteTimer = Stopwatch.StartNew();

            if (this.RunSuiteMethods(this.beforeSuites))
            {
                foreach (Test test in this.tests)
                {
                    this.ProcessTest(test);
                }
            }
            else
            {
                this.Skip(string.Empty);
            }

            this.RunSuiteMethods(this.afterSuites);

            suiteTimer.Stop();
            this.Outcome.ExecutionTime = suiteTimer.Elapsed;
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
                test.Skip();
                Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Outcome.Title}' {test.Outcome.Result}");
                this.Outcome.TestsOutcomes.Add(test.Outcome);
            }

            this.Outcome.Result = Status.Skipped;

            try
            {
                OnSuiteSkip?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnSuiteSkip event invoke" + Environment.NewLine + e);
            }
        }

        private void ProcessTest(Test test)
        {
            var dependsOnAttribute = test.TestMethod.GetCustomAttribute(typeof(DependsOnAttribute), true) as DependsOnAttribute;

            if (dependsOnAttribute != null)
            {
                var failedMainTest = this.tests
                    .Where(t => !t.Outcome.Result.Equals(Status.Passed) 
                    && t.TestMethod.Name.Equals(dependsOnAttribute.TestMethod));

                if (failedMainTest.Any())
                {
                    if (Config.DependentTests.Equals(TestsDependency.Skip))
                    {
                        test.Skip();
                        this.Outcome.TestsOutcomes.Add(test.Outcome);
                    }

                    if (!Config.DependentTests.Equals(TestsDependency.Run))
                    {
                        return;
                    }
                }
            }

            var testThread = new Thread(() => this.RunTest(test));
            testThread.Start();

            if (!testThread.Join(Config.TestTimeout))
            {
                testThread.Abort();
                test.Fail(new TimeoutException(string.Format("Test timeout ({0:F1} minutes) reached", Config.TestTimeout.TotalMinutes)));
            }

            this.Outcome.TestsOutcomes.Add(test.Outcome);
        }

        /// <summary>
        /// Run specified Test.
        /// </summary>
        /// <param name="test"><see cref="Test"/> instance</param>
        private void RunTest(Test test)
        {
            try
            {
                if (this.skipTests)
                {
                    test.Skip();
                    Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Outcome.Title}' {Outcome.Result}");
                    return;
                }

                if (!this.RunSuiteMethods(this.beforeTests))
                {
                    test.Skip();
                    Logger.Instance.Log(LogLevel.Info, $"TEST '{test.Outcome.Title}' {Outcome.Result}");
                    return;
                }

                test.Execute(this);

                this.RunAftertests(test.Outcome.Result == Status.Failed);

                if (test.Outcome.Result == Status.Failed)
                {
                    this.Outcome.Result = Status.Failed;

                    if (test.Outcome.Defect != null)
                    {
                        this.Outcome.Bugs.Add(test.Outcome.Defect);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                // In case of test fail by timeout.
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
                    skipTests = attribute.SkipTestsOnFail && Config.ParallelBy != Parallelization.Test;
                }
            }
        }

        /// <summary>
        /// Get list of Tests from suite instance based on [Test] Attribute presence. 
        /// Determine if test should be skipped and update runnable tests count for the suite. 
        /// </summary>
        /// <returns>array of <see cref="Test"/> instances</returns>
        private Test[] GetTests()
        {
            List<Test> testMethods = new List<Test>();

            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(typeof(TestAttribute), true) != null)
                .Where(m => AdapterUtilities.IsTestRunnable(m));

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
        /// <returns><see cref="Test"/> instance</returns>
        private Test GenerateTest(MethodInfo method, DataSet dataSet)
        {
            var test = dataSet == null ? new Test(method) : new Test(method, dataSet);
             
            test.MethodType = SuiteMethodType.Test;
            test.Outcome.ParentId = this.Id;
            return test;
        }

        /// <summary>
        /// Get list of MethodInfo from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attributeType">Type of attribute</param>
        /// <param name="type">type of suite method (<see cref="SuiteMethodType"/>)</param>
        /// <returns>array of <see cref="SuiteMethod"/> with specified attribute</returns>
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
                    suiteMethod.Outcome.ParentId = this.Id;
                    suiteMethod.MethodType = type;
                    suitableMethods.Add(suiteMethod);
                }
            }

            return suitableMethods.ToArray();
        }

        #endregion
    }
}
