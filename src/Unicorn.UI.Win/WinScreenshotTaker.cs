using System;
using System.Drawing;
using System.Drawing.Imaging;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core;
using Unicorn.UI.Win.WindowsApi;

namespace Unicorn.UI.Win
{
    /// <summary>
    /// Provides ability to take screenshots.
    /// </summary>
    public class WinScreenshotTaker : ScreenshotTakerBase, IDisposable
    {
        private const string LogPrefix = nameof(WinScreenshotTaker);
        private const int MaxLength = 255;

        private readonly ImageFormat _format;
        private readonly Size _screenSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinScreenshotTaker"/> class with default directory.<para/>
        /// Default directory is ".\Screenshots" (created automatically if it does not exist).
        /// </summary>
        public WinScreenshotTaker() : this(
            DefaultDirectory,
            System.Drawing.Imaging.ImageFormat.Png)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinScreenshotTaker"/> class based on specified directory and image format.<para/>
        /// Directory is created automatically if it does not exist.
        /// </summary>
        /// <param name="screenshotsDir">directory to save screenshots to</param>
        /// <param name="format">screenshot image format</param>
        public WinScreenshotTaker(string screenshotsDir, ImageFormat format) : base(screenshotsDir)
        {
            _format = format;
            ImageFormat = _format.ToString();
            _screenSize = Screen.GetSize();
        }

        /// <summary>
        /// Gets image format string.
        /// </summary>
        protected override string ImageFormat { get; }

        /// <summary>
        /// Take screenshot with specified name and save to specified directory.
        /// </summary>
        /// <param name="folder">folder to save screenshot to</param>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        public string TakeScreenshot(string folder, string fileName)
        {
            Bitmap printScreen = GetScreenshot();

            if (printScreen == null)
            {
                return string.Empty;
            }

            try
            {
                ULog.Debug(LogPrefix + ": Saving print screen...");
                string filePath = BuildFileName(folder, fileName);
                printScreen.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                return filePath;
            }
            catch (Exception e)
            {
                ULog.Warn(LogPrefix + ": Failed to save print screen: {0}", e);
                return string.Empty;
            }
        }

        /// <summary>
        /// Take screenshot with specified name and save to screenshots directory.
        /// </summary>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        public override string TakeScreenshot(string fileName) => TakeScreenshot(ScreenshotsDir, fileName);

        /// <summary>
        /// Unsubscribes screenshotter from taf events if was subscribed.
        /// </summary>
        public void Dispose() =>
            UnsubscribeFromTafEvents();

        private Bitmap GetScreenshot()
        {
            try
            {
                ULog.Debug(LogPrefix + ": Creating print screen...");

                Bitmap captureBmp = new Bitmap(_screenSize.Width, _screenSize.Height, PixelFormat.Format32bppArgb);
                using (Graphics captureGraphic = Graphics.FromImage(captureBmp))
                {
                    captureGraphic.CopyFromScreen(0, 0, 0, 0, captureBmp.Size);
                    return captureBmp;
                }
            }
            catch (Exception e)
            {
                ULog.Warn(LogPrefix + ": Failed to get print screen: {0}", e);
                return null;
            }
        }
    }
}
