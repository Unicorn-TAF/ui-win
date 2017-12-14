using NUnit.Framework;
using ProjectSpecific.Util;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;

namespace ProjectSpecific
{
    [TestFixture]
    public class NUnitReportPortalTestRunner
    {
        [OneTimeSetUp]
        public static void ClassInit()
        {
            Logger.Instance = new Util.ConsoleLogger();
            Logger.Instance.Init();
            Reporter.Instance = new ReportPortalReporter();
            Reporter.Instance.Init();
        }

        [OneTimeTearDown]
        public static void ClassTearDown()
        {
            Reporter.Instance.Complete();
        }
    }
}
