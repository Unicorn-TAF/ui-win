using NUnit.Framework;
using ProjectSpecific.Util;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Tests.TestData;

namespace Tests.UnitTests
{
    [TestFixture]
    public class PlatformsMixTests
    {

        [TestCase]
        public void PlatformMixTest()
        {
            Logger.Instance = new ConsoleLogger();
            Reporter.Instance = new ExcelReporter();
            PlatformsMixSuite suite = new PlatformsMixSuite();
            suite.Run();
        }
    }
}
