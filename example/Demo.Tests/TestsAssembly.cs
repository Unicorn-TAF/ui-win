using Demo.Specifics.Environment;
using System.Drawing.Imaging;
using System.IO;
using Unicorn.AllureAgent;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Utility;

namespace Demo.Tests
{
    /// <summary>
    /// Actions performed before and/or after all tests execution.
    /// </summary>
    [TestAssembly]
    public class TestsAssembly
    {
        ////private static ReportPortalReporterInstance reporter;
        private static AllureReporterInstance reporter;
        private static Screenshotter screenshotter;

        /// <summary>
        /// Actions before all tests execution.
        /// The method should be static.
        /// </summary>
        [RunInitialize]
        public static void InitRun()
        {
            // Use of custom logger instead of default Console logger.
            Logger.Instance = new FileLogger();

            // Set trace logging level.
            Logger.Level = LogLevel.Trace;

            // Initialize built-in screenshotter with automatic subscription to test fail event.
            var screenshotsDir = Path.Combine(Config.Instance.TestsDir, "Screenshots");
            screenshotter = new Screenshotter(screenshotsDir, ImageFormat.Png, true);

            // Initialize built-in allure reporter with automatic subscription to all testing events.
            reporter = new AllureReporterInstance();
            ////reporter = new ReportPortalReporterInstance();
        }

        /// <summary>
        /// Actions after all tests execution.
        /// The method should be static.
        /// </summary>
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
