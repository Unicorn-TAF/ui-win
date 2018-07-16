using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests.Attributes;
using Unicorn.ReportPortalAgent;

namespace Tests.TestData
{
    [TestsAssembly]
    public class UnicornTestsRunBase
    {
        [RunInitialize]
        public static void InitRun()
        {
            Logger.Instance = new ProjectSpecific.Util.ConsoleLogger();
            Reporter.Instance = new ReporterInstance();
            Reporter.Instance.Init();
        }

        [RunFinalize]
        public static void FinalizeRun()
        {
            Reporter.Instance.Complete();
        }
    }
}
