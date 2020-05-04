using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.ReportPortalAgent;
using Unicorn.Taf.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using Demo.Specifics.Environment;

namespace Demo.Tests
{
    [TestAssembly]
    public class TestsAssembly
    {
        private static ReportPortalReporterInstance reporter;
        private static Screenshotter screenshotter;

        [RunInitialize]
        public static void InitRun()
        {
            var screenshotsDir = Path.Combine(Config.Instance.TestsDir, "Screenshots");
            screenshotter = new Screenshotter(screenshotsDir, ImageFormat.Png, true);
            reporter = new ReportPortalReporterInstance();
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
