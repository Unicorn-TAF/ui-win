using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class TestTimeouts : NUnitTestRunner
    {
        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.SetSuiteTags(Tag.Timeouts);
            Config.TestTimeout = TimeSpan.FromSeconds(0.5);
            runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
        }

        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Test timeout")]
        public void TestTimeoutForTest()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 1", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.FailedTests, Is.EqualTo(1));
            Assert.That(outcome.TestsOutcomes.First(o => o.Result.Equals(Status.Failed)).Exception.GetType(), Is.EqualTo(typeof(TestTimeoutException)));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BeforeSuite timeout")]
        public void TestTimeoutForBeforeSuite()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 2", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.Result, Is.EqualTo(Status.Skipped));
            Assert.That(outcome.SkippedTests, Is.EqualTo(2));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BeforeTest timeout")]
        public void TestTimeoutForBeforeTest()
        {
            var outcome = runner.Outcome.SuitesOutcomes.First(o => o.Name.Equals("Suite for timeouts 3", StringComparison.InvariantCultureIgnoreCase));

            Assert.That(outcome.Result, Is.EqualTo(Status.Failed));
            Assert.That(outcome.SkippedTests, Is.EqualTo(1));
            Assert.That(outcome.PassedTests, Is.EqualTo(1));
        }
    }
}
