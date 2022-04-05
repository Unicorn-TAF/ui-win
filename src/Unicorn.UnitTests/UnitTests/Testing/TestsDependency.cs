using NUnit.Framework;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestsDependency : NUnitTestRunner
    {
        [OneTimeSetUp]
        public static void SetConfig()
        {
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            Config.SetSuiteTags("dependencies");
        }

        [OneTimeTearDown]
        public static void ResetConfig() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests don't run on TestsDependency.DoNotRun")]
        public void TestDependentTestsDontRun()
        {
            Config.DependentTests = Taf.Core.TestsDependency.DoNotRun;
            
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
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
            Config.DependentTests = Taf.Core.TestsDependency.Skip;

            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes;

            Assert.That(testOutcomes.Count, Is.EqualTo(6));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Failed test depending on failed test")).Result, 
                Is.EqualTo(Status.Skipped));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 3")).Result, 
                Is.EqualTo(Status.Skipped));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test with fail")).Result,
                Is.EqualTo(Status.Failed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 1")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 2")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 4")).Result,
                Is.EqualTo(Status.Passed));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dependent tests run on TestsDependency.Run")]
        public void TestDependentTestsRun()
        {
            Config.DependentTests = Taf.Core.TestsDependency.Run;

            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            var testOutcomes = runner.Outcome.SuitesOutcomes[0].TestsOutcomes;

            Assert.That(testOutcomes.Count, Is.EqualTo(6));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Failed test depending on failed test")).Result,
                Is.EqualTo(Status.Failed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 3")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test with fail")).Result,
                Is.EqualTo(Status.Failed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 1")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 2")).Result,
                Is.EqualTo(Status.Passed));

            Assert.That(
                testOutcomes.First(o => o.Title.Equals("Test 4")).Result,
                Is.EqualTo(Status.Passed));
        }
    }
}
