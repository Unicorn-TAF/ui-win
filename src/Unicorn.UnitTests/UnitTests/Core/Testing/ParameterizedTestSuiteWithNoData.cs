using NUnit.Framework;
using System.Reflection;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Testing
{
    [TestFixture]
    public class ParameterizedTestSuiteWithNoData : NUnitTestRunner
    {
        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that parameterized test suite without data does not run tests")]
        public void TestParameterizedSuiteWithNoDataDoesNotRunTests()
        {
            Config.SetSuiteTags(Tag.ParameterizedBroken);
            var runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }
    }
}
