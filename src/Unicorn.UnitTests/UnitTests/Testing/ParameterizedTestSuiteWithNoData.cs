using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class ParameterizedTestSuiteWithNoData : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that parameterized test suite without data does not run tests")]
        public void TestParameterizedSuiteWithNoDataDoesNotRunTests()
        {
            Config.Reset();
            Config.SetSuiteTags("parameterizedBroken");
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }
    }
}
