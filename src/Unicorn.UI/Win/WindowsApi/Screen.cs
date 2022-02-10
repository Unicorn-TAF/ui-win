using System.Drawing;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Win.WindowsApi
{
    public static class Screen
    {
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
