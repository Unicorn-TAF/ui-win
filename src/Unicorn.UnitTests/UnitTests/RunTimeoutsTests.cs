using System;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Core.Engine;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class RunTimeoutsTests : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check Test timeout")]
        public void TestTimeoutsTestTimeout()
        {
            Configuration.SetSuiteFeatures("timeouts");
            Configuration.TestTimeout = TimeSpan.FromSeconds(1);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            Assert.That(runner.Outcome.SuitesOutcomes[0].FailedTests, Is.EqualTo(1));
        }
    }
}
