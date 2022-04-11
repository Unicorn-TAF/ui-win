using System.Drawing.Imaging;
using System.IO;
using Unicorn.AllureAgent;
using Unicorn.ReportPortalAgent;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UI.Win;

namespace Demo.Tests
{
    /// <summary>
    /// Actions performed before and/or after all tests execution.
    /// </summary>
    [TestAssembly]
    public class TestsAssembly
    {
        private static AllureReporterInstance reporter;
        ////private static ReportPortalReporterInstance rpReporter;
        private static WinScreenshotTaker screenshotter;

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

#if NETFRAMEWORK
            // Initialize built-in screenshotter with automatic subscription to test fail event.
            var screenshotsDir = Path.Combine(Config.Instance.TestsDir, "Screenshots");
            screenshotter = new WinScreenshotTaker(screenshotsDir, ImageFormat.Png);
            screenshotter.ScribeToTafEvents();
#endif

            // Initialize built-in allure reporter with automatic subscription to all testing events.
            // allureConfig.json should exist in binaries directory.
            reporter = new AllureReporterInstance();

            // Initialize built-in report portal reporter with automatic subscription to all testing events.
            // ReportPortal.config.json should exist in binaries directory.
            ////rpReporter = new ReportPortalReporterInstance();
        }

        /// <summary>
        /// Actions after all tests execution.
        /// The method should be static.
        /// </summary>
        [RunFinalize]
        public static void FinalizeRun()
        {
            // Unsubscribe allure reporter from unicorn events.
            reporter.Dispose();

            // Unsubscribe report portal reporter from unicorn events.
            ////rpReporter.Dispose();
#if NETFRAMEWORK
            // unsubscribing screenshotter from unicorn events.
            screenshotter.UnsubscribeFromTafEvents();
#endif
            reporter = null;
            screenshotter = null;
        }
    }
}
