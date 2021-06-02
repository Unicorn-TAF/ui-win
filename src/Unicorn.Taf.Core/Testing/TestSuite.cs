using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Represents class container of <see cref="Test"/> and <see cref="SuiteMethod"/><br/>
    /// Contains list of events related to different Suite states (started, finished, skipped)<br/>
    /// Could have <see cref="ParameterizedAttribute"/> (the class should contain parameterized constructor with corresponding parameters)<para/>
    /// Each class with tests should inherit from <see cref="TestSuite"/>
    /// </summary>
    public class TestSuite
    {
        private readonly Test[] _tests;
        private readonly SuiteMethod[] _beforeSuites;
        private readonly SuiteMethod[] _beforeTests;
        private readonly SuiteMethod[] _afterTests;
        private readonly SuiteMethod[] _afterSuites;

        private HashSet<string> tags = null;
        private bool skipTests = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestSuite"/> class.<br/>
        /// On Initialize the list of Tests, BeforeTests, AfterTests, BeforeSuites and AfterSuites 
        /// is retrieved from the instance.<br/>
        /// For each test is performed check for skip
        /// </summary>
        public TestSuite()
        {
            Metadata = new Dictionary<string, string>();

            foreach (var attribute in GetType().GetCustomAttributes(typeof(MetadataAttribute), true) as MetadataAttribute[])
            {
                if (!Metadata.ContainsKey(attribute.Key))
                {
                    Metadata.Add(attribute.Key, attribute.Value);
                }
            }

            var suiteAttribute = GetType().GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute;

            Outcome = new SuiteOutcome
            {
                Name = suiteAttribute != null ? suiteAttribute.Name : GetType().Name.Split('.').Last(),
                Id = Guid.NewGuid(),
                Result = Status.NotExecuted
            };

            _beforeSuites = SuiteUtilities
                .GetSuiteMethodsFrom(this, typeof(BeforeSuiteAttribute), SuiteMethodType.BeforeSuite);
            _beforeTests = SuiteUtilities
                .GetSuiteMethodsFrom(this, typeof(BeforeTestAttribute), SuiteMethodType.BeforeTest);
            _afterTests = SuiteUtilities
                .GetSuiteMethodsFrom(this, typeof(AfterTestAttribute), SuiteMethodType.AfterTest);
            _afterSuites = SuiteUtilities
                .GetSuiteMethodsFrom(this, typeof(AfterSuiteAttribute), SuiteMethodType.AfterSuite);
            
            _tests = SuiteUtilities.GetTestsFrom(this);
        }

        /// <summary>
        /// Delegate used for suite events invocation
        /// </summary>
        /// <param name="testSuite">current <see cref="TestSuite"/> instance</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void UnicornSuiteEvent(TestSuite testSuite);

        /// <summary>
        /// Event is invoked before suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteStart;

        /// <summary>
        /// Event is invoked after suite execution
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteFinish;

        /// <summary>
        /// Event is invoked if suite is skipped
        /// </summary>
        public static event UnicornSuiteEvent OnSuiteSkip;

        /// <summary>
        /// Gets test suite features. Suite could not have any feature
        /// </summary>
        public HashSet<string> Tags
        {
            get
            {
                if (tags == null)
                {
                    var attributes = GetType().GetCustomAttributes(typeof(TagAttribute), true) as TagAttribute[];
                    tags = new HashSet<string>(from attribute in attributes select attribute.Tag.ToUpper());
                }

                return tags;
            }
        }

        /// <summary>
        /// Gets TestSuite metadata dictionary, can contain only string values
        /// </summary>
        public Dictionary<string, string> Metadata { get; }

        /// <summary>
        /// Gets suite name (if parameterized data set name ignored)
        /// </summary>
        public string Name => Outcome.Name;

        /// <summary>
        /// Gets or sets Suite outcome, contain all information on suite run and results
        /// </summary>
        public SuiteOutcome Outcome { get; protected set; }

        internal Stopwatch ExecutionTimer { get; private set; }

        internal void Execute()
        {
            var fullName = Outcome.Name;

            if (!string.IsNullOrEmpty(Outcome.DataSetName))
            {
                fullName += "[" + Outcome.DataSetName + "]";
            }

            Logger.Instance.Log(LogLevel.Info, $"---------------- Suite '{fullName}'");

            var onSuiteStartPassed = false;

            try
            {
                OnSuiteStart?.Invoke(this);
                onSuiteStartPassed = true;
            }
            catch (Exception ex)
            {
                Skip("Exception occured during " + nameof(OnSuiteStart) + " event invoke" + Environment.NewLine + ex);
            }

            if (onSuiteStartPassed)
            {
                RunSuite();
            }

            try
            {
                OnSuiteFinish?.Invoke(this);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, 
                    "Exception occured during " + nameof(OnSuiteFinish) + " event invoke" + Environment.NewLine + ex);
            }

            Logger.Instance.Log(LogLevel.Info, $"Suite {Outcome.Result}");
        }

        private void RunSuite()
        {
            ExecutionTimer = Stopwatch.StartNew();

            if (RunSuiteMethods(_beforeSuites))
            {
                foreach (Test test in _tests)
                {
                    ProcessTest(test);
                }

                Outcome.Result = Outcome.TestsOutcomes.All(to => to.Result == Status.Passed) ?
                    Status.Passed :
                    Status.Failed;
            }
            else
            {
                Skip(string.Empty);
            }

            RunSuiteMethods(_afterSuites);

            ExecutionTimer.Stop();
            Outcome.ExecutionTime = ExecutionTimer.Elapsed;
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        private void Skip(string reason)
        {
            Logger.Instance.Log(LogLevel.Info, reason);

            foreach (Test test in _tests)
            {
                test.Skip();
                Logger.Instance.Log(LogLevel.Warning, $"Test '{test.Outcome.Title}' {test.Outcome.Result}");
                Outcome.TestsOutcomes.Add(test.Outcome);
            }

            Outcome.Result = Status.Skipped;

            try
            {
                OnSuiteSkip?.Invoke(this);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, 
                    "Exception occured during " + nameof(OnSuiteSkip) + " event invoke" + Environment.NewLine + e);
            }
        }

        private void ProcessTest(Test test)
        {
            var dependsOnAttribute = test.TestMethod
                .GetCustomAttribute(typeof(DependsOnAttribute), true) as DependsOnAttribute;

            if (dependsOnAttribute != null)
            {
                var failedMainTest = _tests
                    .Where(t => !t.Outcome.Result.Equals(Status.Passed) 
                    && t.TestMethod.Name.Equals(dependsOnAttribute.TestMethod));

                if (failedMainTest.Any())
                {
                    if (Config.DependentTests.Equals(TestsDependency.Skip))
                    {
                        test.Skip();
                        Outcome.TestsOutcomes.Add(test.Outcome);
                    }

                    if (!Config.DependentTests.Equals(TestsDependency.Run))
                    {
                        return;
                    }
                }
            }

            RunTest(test);

            Outcome.TestsOutcomes.Add(test.Outcome);
        }

        /// <summary>
        /// Run specified <see cref="Test"/>.
        /// </summary>
        /// <param name="test"><see cref="Test"/> instance</param>
        private void RunTest(Test test)
        {
            if (skipTests || ExecutionTimer.Elapsed >= Config.SuiteTimeout || !RunSuiteMethods(_beforeTests))
            {
                test.Skip();
                Logger.Instance.Log(LogLevel.Warning, $"Test '{test.Outcome.Title}' {test.Outcome.Result}");
                return;
            }

            test.Execute(this);

            RunAftertests(test.Outcome.Result == Status.Failed);

            if (test.Outcome.Result == Status.Failed)
            {
                Outcome.Result = Status.Failed;
            }
        }

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

                if (suiteMethod.Outcome.Result == Status.Failed)
                {
                    Outcome.Result = Status.Failed;
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
            foreach (var suiteMethod in _afterTests)
            {
                var attribute = suiteMethod.TestMethod
                    .GetCustomAttribute(typeof(AfterTestAttribute), true) as AfterTestAttribute;

                if (testWasFailed && !attribute.RunAlways)
                {
                    return;
                }

                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result == Status.Failed)
                {
                    Outcome.Result = Status.Failed;
                    //TODO: && Config.ParallelBy != Parallelization.Test;
                    skipTests = attribute.SkipTestsOnFail;
                }
            }
        }
    }
}
