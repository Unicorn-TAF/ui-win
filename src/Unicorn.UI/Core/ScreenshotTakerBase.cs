using System;
using System.IO;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.UI.Core
{
    /// <summary>
    /// Base class for specific UI screenshotters implementations. Contains base properties, constructors and methods.
    /// </summary>
    public abstract class ScreenshotTakerBase
    {
        private const int MaxPathLength = 250;

        /// <summary>
        /// Gets default screenshots directory name (binaries_dir\Screenshots)
        /// </summary>
        protected static readonly string DefaultDirectory = Path.Combine(
            Path.GetDirectoryName(new Uri(typeof(ScreenshotTakerBase).Assembly.Location).LocalPath),
            "Screenshots");

        /// <summary>
        /// Initializes a new instance of the <see cref="ScreenshotTakerBase"/> class with screenshots directory.
        /// </summary>
        /// <param name="screenshotsDir"></param>
        protected ScreenshotTakerBase(string screenshotsDir)
        {
            ScreenshotsDir = screenshotsDir;

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }
        }

        /// <summary>
        /// Gets screenshots directory name.
        /// </summary>
        public string ScreenshotsDir { get; }

        /// <summary>
        /// Gets image format string.
        /// </summary>
        protected abstract string ImageFormat { get; }

        /// <summary>
        /// Subscribe to Unicorn events.
        /// </summary>
        public void SubcribeToTafEvents()
        {
            Test.OnTestFail += TakeScreenshot;
            SuiteMethod.OnSuiteMethodFail += TakeScreenshot;
        }

        /// <summary>
        /// Unsubscribe from Unicorn events.
        /// </summary>
        public void UnsubscribeFromTafEvents()
        {
            Test.OnTestFail -= TakeScreenshot;
            SuiteMethod.OnSuiteMethodFail -= TakeScreenshot;
        }

        /// <summary>
        /// Takes screenshot with specified name and saves to screenshots directory.
        /// </summary>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>full path to the screenshot file</returns>
        public abstract string TakeScreenshot(string fileName);

        /// <summary>
        /// Generates full file name for the screenshot. If target file name is longer than 250 symbols 
        /// it's trimmed and ended with "~" + number of trimmed symbols.
        /// </summary>
        /// <param name="directory">screenshots directory</param>
        /// <param name="fileName">screen file name without extension</param>
        /// <returns>full file path</returns>
        /// <exception cref="PathTooLongException"> is thrown when directory name is longer than 250 symbols</exception>
        protected string BuildFileName(string directory, string fileName)
        {
            if (directory.Length >= MaxPathLength)
            {
                throw new PathTooLongException("Directory name is too long, please specify another location.");
            }

            string filePath = Path.Combine(directory, fileName);

            int overhead = filePath.Length - MaxPathLength;

            if (overhead > 0)
            {
                filePath = filePath.Substring(0, MaxPathLength - 2) + "~" + (overhead + 2);
            }

            filePath += "." + ImageFormat;

            return filePath;
        }

        private void TakeScreenshot(SuiteMethod suiteMethod)
        {
            string mime = "image/" + ImageFormat.ToLowerInvariant();
            string screenshotPath = TakeScreenshot(suiteMethod.Outcome.FullMethodName);

            suiteMethod.Outcome.Attachments.Add(new Attachment("Screenshot", mime, screenshotPath));
        }
    }
}
