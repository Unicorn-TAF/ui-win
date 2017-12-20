using NUnit.Framework;
using ProjectSpecific;
using Tests.TestData;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestsPlatformsMix : NUnitReportPortalTestRunner
    {
        [Ignore("")]
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test to check Demo version of TestSuite")]
        public void PlatformMixTest()
        {
            PlatformsMixSuite suite = new PlatformsMixSuite();
            suite.Run();
            if (suite.Outcome.Result == Unicorn.Core.Testing.Tests.Result.FAILED)
            {
                Assert.Fail();
            }
        }
    }
}
