using System;

namespace Unicorn.UI.Win.WindowsApi
{
    [Flags]
    internal enum KeyboardFlag : uint
    {
        KEYEVENTF_KEYDOWN = 0x0000,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
        KEYEVENTF_UNICODE = 0x0004,
        KEYEVENTF_SCANCODE = 0x0008
    }

    internal struct KEYBDINPUT
    {
        internal UInt16 Vk;
        internal UInt16 Scan;
        internal UInt32 Flags;
        internal UInt32 Time;
        internal IntPtr ExtraInfo;

        internal KEYBDINPUT(UInt16 vk, UInt32 flags)
        {
            Vk = vk;
            Scan = 0;
            Flags = flags;
            Time = 0;
            ExtraInfo = IntPtr.Zero;
        }
    }
}
