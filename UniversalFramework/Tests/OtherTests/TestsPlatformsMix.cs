using NUnit.Framework;
using ProjectSpecific;
using System.Reflection;
using Tests.TestData;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestsPlatformsMix : NUnitReportPortalTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test to check Demo version of TestSuite")]
        public void PlatformMixTest()
        {
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly());
            runner.RunTests();
        }
    }
}
