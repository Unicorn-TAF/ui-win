using System.Drawing;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Win.WindowsApi
{
    /// <summary>
    /// Screen related utility.
    /// </summary>
    public static class Screen
    {
        /// <summary>
        /// Gets current screen size.
        /// </summary>
        /// <returns><see cref="Size"/> of the screen</returns>
        public static Size GetSize()
        {
            const int ENUM_CURRENT_SETTINGS = -1;

            DEVMODE devMode = default;
            devMode.dmSize = (short)Marshal.SizeOf(devMode);
            NativeMethods.EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref devMode);

            return new Size(devMode.dmPelsWidth, devMode.dmPelsHeight);
        }
    }
}
