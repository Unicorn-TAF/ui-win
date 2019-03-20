using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Reporting
{
    public static class Screenshot
    {
        private const int MaxLength = 255;
        private static ImageFormat format = ImageFormat.Png;

        public static string ScreenshotsFolder { get; set; } = Path.Combine(Path.GetDirectoryName(new Uri(typeof(Screenshot).Assembly.CodeBase).LocalPath), "Screenshots");

        public static Bitmap GetScreenshot()
        {
            Bitmap printScreen;
            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Creating print screen...");

                int screenLeft = SystemInformation.VirtualScreen.Left;
                int screenTop = SystemInformation.VirtualScreen.Top;
                int screenWidth = SystemInformation.VirtualScreen.Width;
                int screenHeight = SystemInformation.VirtualScreen.Height;

                printScreen = new Bitmap(screenWidth, screenHeight);
                using (Graphics g = Graphics.FromImage(printScreen))
                {
                    g.CopyFromScreen(screenLeft, screenTop, 0, 0, printScreen.Size);
                }
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Debug, "Failed to get print screen:\n" + e);
                printScreen = new Bitmap(1, 1);
            }

            return printScreen;
        }

        public static string TakeScreenshot(string folder, string fileName)
        {
            var printScreen = GetScreenshot();
            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Saving print screen");
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
                Logger.Instance.Log(LogLevel.Warning, "Failed to save print screen:\n" + e);
                return string.Empty;
            }
        }

        public static string TakeScreenshot(string fileName) => TakeScreenshot(ScreenshotsFolder, fileName);
    }
}
