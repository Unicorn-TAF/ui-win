using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestSuiteOutcome : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static TimeSpan runTime;

        [OneTimeSetUp]
        public static void SetUp()
        {
            Config.SetSuiteTags("sample");
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);

            var timer = Stopwatch.StartNew();
            runner.RunTests();

            runTime = timer.Elapsed;
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            Config.Reset();
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Author attribute")]
        public void TestAuthorAttribute()
        {
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.First(o => o.Title.Equals("Test1")).Author, Is.EqualTo("No author"));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.First(o => o.Title.Equals("Test2")).Author, Is.EqualTo("Author2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite outcome TotalTests counter")]
        public void TestSuiteOutcomeTotalTestsCounter() =>
            Assert.That(runner.Outcome.SuitesOutcomes[0].TotalTests, Is.EqualTo(2));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check suite outcome ExecutionTime")]
        public void TestSuiteOutcomeExecutionTime()
        {
            Assert.That(runner.Outcome.SuitesOutcomes[0].ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(runner.Outcome.SuitesOutcomes[0].ExecutionTime, Is.Not.GreaterThan(runTime));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].ExecutionTime, Is.Not.GreaterThan(runTime));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[1].ExecutionTime, Is.GreaterThan(TimeSpan.Zero));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[1].ExecutionTime, Is.Not.GreaterThan(runTime));
        }
    }
}
