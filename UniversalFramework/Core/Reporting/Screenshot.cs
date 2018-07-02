using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Reporting
{
    public class Screenshot
    {
        public static string ScreenshotsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");

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
                Logger.Instance.Log(LogLevel.Debug, "Failed to get print screen:\n" + e.ToString());
                printScreen = new Bitmap(1, 1);
            }

            return printScreen;
        }

        public static void TakeScreenshot(string fileName)
        {
            Bitmap printScreen = GetScreenshot();
            try
            {
                Logger.Instance.Log(LogLevel.Debug, "Saving print screen");
                printScreen.Save(Path.Combine(ScreenshotsFolder, fileName + "." + ImageFormat.Jpeg), ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                Logger.Instance.Log(LogLevel.Debug, "Failed to save print screen:\n" + e.ToString());
            }
        }
    }
}
