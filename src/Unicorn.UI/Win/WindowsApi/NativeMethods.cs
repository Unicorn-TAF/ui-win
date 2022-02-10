using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Win.WindowsApi
{
    internal static class NativeMethods
    {
        [DllImport("user32", EntryPoint = "SendInput")]
        internal static extern int SendInput(uint numberOfInputs, ref Input input, int structSize);

        [DllImport("user32.dll")]
        internal static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        internal static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        internal static extern ushort GetKeyState(uint virtKey);

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(ref Point cursorInfo);

        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern short GetDoubleClickTime();

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplaySettings(string deviceName, int modeNum, ref DEVMODE devMode);
    }
}
