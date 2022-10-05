using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Internal;
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

            foreach (var attribute in GetType().GetCustomAttributes<MetadataAttribute>(true))
            {
                if (!Metadata.ContainsKey(attribute.Key))
                {
                    Metadata.Add(attribute.Key, attribute.Value);
                }
            }

            var suiteAttribute = GetType().GetCustomAttribute<SuiteAttribute>(true);

            Outcome = new SuiteOutcome
            {
                Name = suiteAttribute != null ? suiteAttribute.Name : GetType().Name.Split('.').Last(),
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
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
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
                    var attributes = GetType().GetCustomAttributes<TagAttribute>(true);
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
            // First need to generate full name including dataset as it influences IDs generation.
            string fullName = string.IsNullOrEmpty(Outcome.DataSetName) ?
                Outcome.Name : 
                $"{Outcome.Name}[{Outcome.DataSetName}]";

            GenerateIds();

            ULog.Info("---------------- Suite '{0}'", fullName);

            TafEvents.ExecuteSuiteEvent(OnSuiteStart, this, nameof(OnSuiteStart));
            ExecutionTimer = Stopwatch.StartNew();

            if (RunSuiteMethods(_beforeSuites))
            {
                Array.ForEach(_tests, t => ProcessTest(t));

                bool allTestsPassed = Outcome.TestsOutcomes.All(to => to.Result == Status.Passed);
                Outcome.Result = allTestsPassed ? Status.Passed : Status.Failed;
            }
            else
            {
                Skip("BeforeSuite was failed.");
            }

            RunSuiteMethods(_afterSuites);
            
            ExecutionTimer.Stop();
            Outcome.ExecutionTime = ExecutionTimer.Elapsed;

            ULog.Info("Suite {0}", Outcome.Result);
            TafEvents.ExecuteSuiteEvent(OnSuiteFinish, this, nameof(OnSuiteFinish));
        }

        /// <summary>
        /// Skip test suite and invoke onSkip event
        /// </summary>
        /// <param name="reason">skip reason message</param>
        private void Skip(string reason)
        {
            ULog.Warn(reason);

            foreach (Test test in _tests)
            {
                test.Skip();
                Outcome.TestsOutcomes.Add(test.Outcome);
            }

            Outcome.Result = Status.Skipped;
            TafEvents.ExecuteSuiteEvent(OnSuiteSkip, this, nameof(OnSuiteSkip));
        }

        private void ProcessTest(Test test)
        {
            var dependsOnAttribute = test.TestMethod.GetCustomAttribute<DependsOnAttribute>(true);

            if (dependsOnAttribute != null)
            {
                bool anyFailedMainTest = _tests
                    .Any(t => !t.Outcome.Result.Equals(Status.Passed) 
                    && t.TestMethod.Name.Equals(dependsOnAttribute.TestMethod));

                if (anyFailedMainTest)
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

            Array.ForEach(_beforeTests, 
                beforeTest => SuiteUtilities.GenerateSuiteMethodIds(beforeTest, Outcome.Id, test.Outcome.Title));

            if (skipTests || ExecutionTimer.Elapsed >= Config.SuiteTimeout || !RunSuiteMethods(_beforeTests))
            {
                test.Skip();
            }
            else
            {
                test.Execute(this);

                RunAftertests(test.Outcome);

                if (test.Outcome.Result == Status.Failed)
                {
                    Outcome.Result = Status.Failed;
                }
            }

            Outcome.TestsOutcomes.Add(test.Outcome);
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
        /// <param name="testOutcome">related test outcome</param>
        private void RunAftertests(TestOutcome testOutcome)
        {
            bool testWasFailed = testOutcome.Result == Status.Failed;

            Array.ForEach(_afterTests,
                afterTest => SuiteUtilities.GenerateSuiteMethodIds(afterTest, Outcome.Id, testOutcome.Title));

            foreach (var suiteMethod in _afterTests)
            {
                var attribute = suiteMethod.TestMethod.GetCustomAttribute<AfterTestAttribute>(true);

                if (testWasFailed && !attribute.RunAlways)
                {
                    return;
                }

                suiteMethod.Execute(this);

                if (suiteMethod.Outcome.Result == Status.Failed)
                {
                    Outcome.Result = Status.Failed;
                    //TODO: parallelization is not implemented yet. 
                    //// && Config.ParallelBy != Parallelization.Test;
                    skipTests = attribute.SkipTestsOnFail;
                }
            }
        }

        private void GenerateIds()
        {
            // (type name + data set name) is unique int terms of assembly.
            Outcome.Id = GuidGenerator.FromString(GetType().Name + Outcome.DataSetName);

            Array.ForEach(_beforeSuites,
                beforeSuite => SuiteUtilities.GenerateSuiteMethodIds(beforeSuite, Outcome.Id));

            Array.ForEach(_tests,
                t => SuiteUtilities.GenerateTestIds(t, Outcome.Id));

            Array.ForEach(_afterSuites,
                afterSuite => SuiteUtilities.GenerateSuiteMethodIds(afterSuite, Outcome.Id));
        }
    }
}
