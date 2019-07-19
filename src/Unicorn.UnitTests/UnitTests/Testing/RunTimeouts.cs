using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class RunTimeouts : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Test timeout")]
        public void TestTimeoutsTestTimeout()
        {
            Config.Reset();
            Config.SetSuiteTags("timeouts");
            Config.TestTimeout = TimeSpan.FromSeconds(1);
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();

            Assert.That(runner.Outcome.SuitesOutcomes[0].FailedTests, Is.EqualTo(1));

            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.First(o => o.Result.Equals(Status.Failed)).Exception.GetType(), Is.EqualTo(typeof(TimeoutException)));
        }
    }
}
