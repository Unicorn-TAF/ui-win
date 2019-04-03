using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class TestsDependencyTests : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests don't run on TestsDependency.DoNotRun")]
        public void TestDependentTestsDontRun()
        {
            Config.SetSuiteTags("dependencies");
            Config.DependentTests = TestsDependency.DoNotRun;
            
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes;

            Assert.That(testOutcomes.Count, Is.EqualTo(4));

            Assert.IsFalse(testOutcomes
                .Any(o => o.Title.Equals("Failed test depending on failed test") || o.Title.Equals("Test 3")));

        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests skipped on TestsDependency.Skip")]
        public void TestDependentTestsSkip()
        {
            Config.SetSuiteTags("dependencies");
            Config.DependentTests = TestsDependency.Skip;

            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes;

            Assert.That(testOutcomes.Count, Is.EqualTo(6));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Failed test depending on failed test")).Result, 
                Is.EqualTo(Status.Skipped));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 3")).Result, 
                Is.EqualTo(Status.Skipped));


            Assert.That(testOutcomes.First(o => o.Title.Equals("Test with fail")).Result,
                Is.EqualTo(Status.Failed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 1")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 2")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 4")).Result,
                Is.EqualTo(Status.Passed));
        }


        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests run on TestsDependency.Run")]
        public void TestDependentTestsRun()
        {
            Config.SetSuiteTags("dependencies");
            Config.DependentTests = TestsDependency.Run;

            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes;

            Assert.That(testOutcomes.Count, Is.EqualTo(6));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Failed test depending on failed test")).Result,
                Is.EqualTo(Status.Failed));


            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 3")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test with fail")).Result,
                Is.EqualTo(Status.Failed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 1")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 2")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(testOutcomes.First(o => o.Title.Equals("Test 4")).Result,
                Is.EqualTo(Status.Passed));
        }
    }
}
