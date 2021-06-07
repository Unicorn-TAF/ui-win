using Demo.Specifics.Environment;
using System.Drawing.Imaging;
using System.IO;
using Unicorn.AllureAgent;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Utility;

namespace Demo.Tests
{
    [TestAssembly]
    public class TestsAssembly
    {
        ////private static ReportPortalReporterInstance reporter;
        private static AllureReporterInstance reporter;
        private static Screenshotter screenshotter;

        [RunInitialize]
        public static void InitRun()
        {
            Logger.Instance = new FileLogger();
            Logger.Level = LogLevel.Trace;
            var screenshotsDir = Path.Combine(Config.Instance.TestsDir, "Screenshots");
            screenshotter = new Screenshotter(screenshotsDir, ImageFormat.Png, true);
            ////reporter = new ReportPortalReporterInstance();
            reporter = new AllureReporterInstance();
        }


        [RunFinalize]
        public static void FinalizeRun()
        {
            reporter.Dispose();
            screenshotter.Unsubscribe();
            reporter = null;
            screenshotter = null;
        }
    }
}
