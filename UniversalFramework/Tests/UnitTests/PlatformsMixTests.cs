using NUnit.Framework;
using Tests.TestData;
using ProjectSpecific;

namespace Tests.UnitTests
{
    [TestFixture]
    public class PlatformsMixTests : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test to check Demo version of TestSuite")]
        public void PlatformMixTest()
        {
            PlatformsMixSuite suite = new PlatformsMixSuite();
            suite.Run();
        }
    }
}
