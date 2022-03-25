using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class SuiteTimeouts : NUnitTestRunner
    {
        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup() =>
            Config.SetSuiteTags("suite-timeouts");

        [OneTimeTearDown]
        public static void ResetConfig() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite timeout during before suite")]
        public void TestSuiteTimeoutDuringBeforeSuite()
        {
            Config.SuiteTimeout = TimeSpan.FromSeconds(1);
            var outcome = RunTestSuite();

            Assert.That(outcome.Result, Is.EqualTo(Status.Skipped), 
                "Fail of BeforeSuite by timeout should skip suite");

            Assert.That(outcome.FailedTests, Is.EqualTo(0), "Skipped suite should have 0 fails");

            Assert.That(outcome.SkippedTests, Is.EqualTo(2), 
                "Skipped suite should have all tests skipped");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite timeout during before test")]
        public void TestSuiteTimeoutDuringBeforeTest()
        {
            Config.SuiteTimeout = TimeSpan.FromSeconds(2);
            var outcome = RunTestSuite();

            Assert.That(outcome.Result, Is.EqualTo(Status.Failed), 
                "Fail of BeforeTest by suite timeout should fail suite");
            
            Assert.That(outcome.SkippedTests, Is.EqualTo(2), 
                "All tests should be sklipped on suite timeout during first BeforeTest");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite timeout during before test")]
        public void TestSuiteTimeoutDuringTest()
        {
            Config.SuiteTimeout = TimeSpan.FromSeconds(4);
            var outcome = RunTestSuite();

            Assert.That(outcome.Result, Is.EqualTo(Status.Failed), 
                "Fail of Test by suite timeout should fail suite");
            
            Assert.That(outcome.FailedTests, Is.EqualTo(1), 
                "Current test at suite timeout should be failed");

            Assert.That(outcome.TestsOutcomes[0].Exception.GetType(), Is.EqualTo(typeof(SuiteTimeoutException)),
                "Outcome of test interrupted by suite timeout does not have right exception");

            Assert.That(outcome.SkippedTests, Is.EqualTo(1), 
                "Not executed tests after suite timeout should be skipped");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite timeout is not reached")]
        public void TestSuiteTimeoutIsNotReached()
        {
            Config.SuiteTimeout = TimeSpan.FromSeconds(15);
            var outcome = RunTestSuite();

            Assert.That(outcome.Result, Is.EqualTo(Status.Passed),
                "Suite not reached timeout should pass");

            Assert.That(outcome.PassedTests, Is.EqualTo(2),
                "There are not passed tests while the suite did not reach timeout");
        }

        private SuiteOutcome RunTestSuite()
        {
            runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            return runner.Outcome.SuitesOutcomes
                .First(o => o.Name.Equals("Suite for timeouts 4", 
                StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
