using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestSuiteTestsFilter : NUnitTestRunner
    {
        [TearDown]
        public static void ResetConfig() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test for test name filter (full name)")]
        public void TestFilterForTestNameFullName()
        {
            Config.SetTestsMasks("Unicorn.UnitTests.Suites.USuite.Test2");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test for test name filter (Regex #1)")]
        public void TestFilterForTestNameRegex1()
        {
            Config.SetTestsMasks("~USuite.Test2");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test for test name filter (Regex #1)")]
        public void TestFilterForTestNameRegex2()
        {
            Config.SetTestsMasks("~USuite.*");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(3));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test for not existing test name filter")]
        public void TestFilterForNotExistingTests()
        {
            Config.SetTestsMasks("~USuite.Test254");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test for complex test name filter")]
        public void TestComplexFilterForTests()
        {
            Config.SetTestsMasks("Unicor~sts.S*es.USuite.Test2*");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(2));
        }
    }
}
