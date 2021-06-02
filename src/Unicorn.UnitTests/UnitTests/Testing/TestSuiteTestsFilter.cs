using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestSuiteTestsFilter : NUnitTestRunner
    {
        [TearDown]
        public static void resetConfig() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test or test name filter (full name)")]
        public void TestFilterForTestNameFullName()
        {
            Config.SetTestsMasks("Unicorn.UnitTests.Suites.USuite.Test2");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test or test name filter (Regex #1)")]
        public void TestFilterForTestNameRegex1()
        {
            Config.SetTestsMasks("~USuite.Test2");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test or test name filter (Regex #1)")]
        public void TestFilterForTestNameRegex2()
        {
            Config.SetTestsMasks("~USuite.*");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(2));
        }
    }
}
