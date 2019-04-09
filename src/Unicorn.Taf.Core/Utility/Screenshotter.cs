using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Utility
{
    public static class Screenshotter
    {
        private const int MaxLength = 255;

        public static ImageFormat Format { get; set; } = ImageFormat.Png;

        public static string ScreenshotsFolder { get; set; } = Path.Combine(Path.GetDirectoryName(new Uri(typeof(Screenshotter).Assembly.CodeBase).LocalPath), "Screenshots");

        public static string TakeScreenshot(string folder, string fileName)
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

                filePath += "." + Format;

                printScreen.Save(filePath, Format);
                return filePath;
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Warning, "Failed to save print screen:" + Environment.NewLine + e);
                return string.Empty;
            }
        }

        public static string TakeScreenshot(string fileName) => TakeScreenshot(ScreenshotsFolder, fileName);

        private static Bitmap GetScreenshot()
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
    }
}
