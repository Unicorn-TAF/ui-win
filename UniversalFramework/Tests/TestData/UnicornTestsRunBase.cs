using ProjectSpecific.Util;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Run initialize")]
    public class UnicornTestsRunBase
    {
        [RunInitialize]
        public static void InitRun()
        {
            Reporter.Instance = new ReportPortalReporter();
            Reporter.Instance.Init();
        }

    }
}
