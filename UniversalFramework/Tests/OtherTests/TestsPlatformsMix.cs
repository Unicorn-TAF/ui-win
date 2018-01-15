using System.Reflection;
using NUnit.Framework;
using ProjectSpecific;
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
