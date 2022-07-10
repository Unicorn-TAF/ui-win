using System;
using System.IO;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core;

namespace Unicorn.UI.Mobile.Base
{
    /// <summary>
    /// Provides ability to take screenshots from mobile phone screen.
    /// </summary>
    public class MobileScreenshotTaker : ScreenshotTakerBase
    {
        private const string LogPrefix = "MobileScreenshotTaker: ";

        private readonly OpenQA.Selenium.ScreenshotImageFormat _format;
        private readonly OpenQA.Selenium.IWebDriver _driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileScreenshotTaker"/> class with screenshots directory.
        /// </summary>
        /// <param name="driver"><see cref="OpenQA.Selenium.IWebDriver"/> instance</param>
        /// <param name="screenshotsDir">directory to save screenshots to</param>
        public MobileScreenshotTaker(OpenQA.Selenium.IWebDriver driver, string screenshotsDir) : base(screenshotsDir)
        {
            _driver = driver;
            _format = OpenQA.Selenium.ScreenshotImageFormat.Png;
            ImageFormat = _format.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileScreenshotTaker"/> class with default directory.<para/>
        /// Default directory is ".\Screenshots" (created automatically if it does not exist).
        /// </summary>
        /// <param name="driver"><see cref="OpenQA.Selenium.IWebDriver"/> instance</param>
        public MobileScreenshotTaker(OpenQA.Selenium.IWebDriver driver) : this(driver, DefaultDirectory)
        {
        }

        /// <summary>
        /// Gets image format string.
        /// </summary>
        protected override string ImageFormat { get; }

        /// <summary>
        /// Takes screenshot and saves by specified path as png file. If target file name is longer than 250 symbols 
        /// it's trimmed and ended with "~" + number of trimmed symbols.
        /// </summary>
        /// <param name="folder">folder to save screenshot to</param>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        /// <exception cref="PathTooLongException"> is thrown when directory name is longer than 250 symbols</exception>
        public string TakeScreenshot(string folder, string fileName)
        {
            OpenQA.Selenium.Screenshot printScreen = GetScreenshot();

            if (printScreen == null)
            {
                return string.Empty;
            }

            try
            {
                Logger.Instance.Log(LogLevel.Debug, LogPrefix + "Saving browser print screen...");
                string filePath = BuildFileName(folder, fileName);
                printScreen.SaveAsFile(filePath, _format);
                return filePath;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, LogPrefix + "Failed to save browser print screen:" + Environment.NewLine + e);
                return string.Empty;
            }
        }

        /// <summary>
        /// Takes screenshot with specified name and saves to screenshots directory.
        /// </summary>
        /// <param name="fileName">screenshot file name without extension</param>
        /// <returns>path to the screenshot file</returns>
        public override string TakeScreenshot(string fileName) => TakeScreenshot(ScreenshotsDir, fileName);

        private OpenQA.Selenium.Screenshot GetScreenshot()
        {
            try
            {
                Logger.Instance.Log(LogLevel.Debug, LogPrefix + "Creating browser print screen...");
                return (_driver as OpenQA.Selenium.ITakesScreenshot).GetScreenshot();
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, LogPrefix + "Failed to get browser print screen:" + Environment.NewLine + e);
                return null;
            }
        }
    }
}
