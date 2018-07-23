using System;
using System.Reflection;
using NUnit.Framework;
using Unicorn.UnitTests.Util;
using Unicorn.Core.Testing.Tests.Adapter;

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
            Configuration.TestTimeout = TimeSpan.FromSeconds(2);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();

            Assert.That(runner.ExecutedSuites[0].Outcome.FailedTests, Is.EqualTo(1));
        }
    }
}
