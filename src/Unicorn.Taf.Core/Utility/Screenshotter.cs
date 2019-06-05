using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.Taf.Core.Utility
{
    public class Screenshotter
    {
        private const int MaxLength = 255;

        private readonly ImageFormat format;
        private readonly string screenshotsDir;

        public Screenshotter()
            : this(Path.Combine(Path.GetDirectoryName(new Uri(typeof(Screenshotter).Assembly.CodeBase).LocalPath), "Screenshots"), ImageFormat.Png)
        {
        }

        public Screenshotter(string screenshotsDir, ImageFormat format)
        {
            this.format = format;
            this.screenshotsDir = screenshotsDir;

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            Test.OnTestFail += TakeScreenshot;
            SuiteMethod.OnSuiteMethodFail += TakeScreenshot;
        }

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

                filePath += "." + format;

                printScreen.Save(filePath, format);
                return filePath;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Failed to save print screen:" + Environment.NewLine + e);
                return string.Empty;
            }
        }

        public string TakeScreenshot(string fileName) => TakeScreenshot(screenshotsDir, fileName);

        private Bitmap GetScreenshot()
        {
            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Creating print screen...");
                var printScreen = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

                using (Graphics g = Graphics.FromImage(printScreen))
                {
                    g.CopyFromScreen(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top, 0, 0, printScreen.Size);
                }

                return printScreen;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Failed to get print screen:" + Environment.NewLine + e);
                return null;
            }
        }

        private void TakeScreenshot(SuiteMethod suiteMethod) =>
            suiteMethod.Outcome.Screenshot = this.TakeScreenshot(suiteMethod.Outcome.FullMethodName);
    }
}
