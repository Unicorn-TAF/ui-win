using System;
using System.Runtime.InteropServices;
using Unicorn.UI.Core.UserInput.WindowsApi;

namespace Unicorn.UI.Core.UserInput
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
        internal static extern bool GetCursorPos(ref System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        internal static extern short GetDoubleClickTime();
    }
}
