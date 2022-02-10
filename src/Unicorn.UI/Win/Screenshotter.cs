using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using Unicorn.UI.Win.WindowsApi;

namespace Unicorn.UI.Win
{
    /// <summary>
    /// Provides ability to take screenshots.
    /// </summary>
    public class Screenshotter
    {
        private const int MaxLength = 255;

        private readonly ImageFormat _format;
        private readonly string _screenshotsDir;
        private readonly Size _screenSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Screenshotter"/> class with default directory.<para/>
        /// Default directory is ".\Screenshots" (created automatically if it does not exist).
        /// </summary>
        public Screenshotter() : this(
            Path.Combine(Path.GetDirectoryName(new Uri(typeof(Screenshotter).Assembly.Location).LocalPath), "Screenshots"), 
            ImageFormat.Png,
            false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screenshotter"/> class based on specified directory and image format.<para/>
        /// Directory is created automatically if it does not exist.
        /// </summary>
        /// <param name="screenshotsDir">directory to save screenshots to</param>
        /// <param name="format">screenshot image format</param>
        public Screenshotter(string screenshotsDir, ImageFormat format) 
            : this(screenshotsDir, format, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Screenshotter"/> class based on specified directory and image format.<para/>
        /// Directory is created automatically if it does not exist.
        /// </summary>
        /// <param name="screenshotsDir">directory to save screenshots to</param>
        /// <param name="format">screenshot image format</param>
        /// <param name="subscribeToEvents">true - if need to subscribe to unicorn events; otherwise - false</param>
        public Screenshotter(string screenshotsDir, ImageFormat format, bool subscribeToEvents)
        {
            _format = format;
            _screenSize = Screen.GetSize();
            _screenshotsDir = screenshotsDir;

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            if (subscribeToEvents)
            {
                Test.OnTestFail += TakeScreenshot;
                SuiteMethod.OnSuiteMethodFail += TakeScreenshot;
            }
        }

        /// <summary>
        /// Take screenshot with specified name and save to specified directory.
        /// </summary>
        /// <param name="folder">folder to save screenshot to</param>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        public string TakeScreenshot(string folder, string fileName)
        {
            var printScreen = GetScreenshot();

            if (printScreen == null)
            {
                return string.Empty;
            }

            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Saving print screen...");
                var filePath = Path.Combine(folder, fileName);

                if (filePath.Length > MaxLength)
                {
                    filePath = filePath.Substring(0, MaxLength - 1) + "~";
                }

                filePath += "." + _format;

                printScreen.Save(filePath, _format);
                return filePath;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Failed to save print screen:" + Environment.NewLine + e);
                return string.Empty;
            }
        }

        /// <summary>
        /// Take screenshot with specified name and save to screenshots directory.
        /// </summary>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        public string TakeScreenshot(string fileName) => TakeScreenshot(_screenshotsDir, fileName);

        /// <summary>
        /// Unsubscribe from Unicorn events.
        /// </summary>
        public void Unsubscribe()
        {
            Test.OnTestFail -= TakeScreenshot;
            SuiteMethod.OnSuiteMethodFail -= TakeScreenshot;
        }

        private Bitmap GetScreenshot()
        {
            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Creating print screen...");

                var captureBmp = new Bitmap(_screenSize.Width, _screenSize.Height, PixelFormat.Format32bppArgb);
                using (Graphics captureGraphic = Graphics.FromImage(captureBmp))
                {
                    captureGraphic.CopyFromScreen(0, 0, 0, 0, captureBmp.Size);
                    return captureBmp;
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Failed to get print screen:" + Environment.NewLine + e);
                return null;
            }
        }

        private void TakeScreenshot(SuiteMethod suiteMethod)
        {
            var mime = "image/" + _format.ToString().ToLowerInvariant();
            var screenshotPath = TakeScreenshot(suiteMethod.Outcome.FullMethodName);

            suiteMethod.Outcome.Attachments.Add(new Attachment("Screenshot", mime, screenshotPath));
        }
            
    }
}
