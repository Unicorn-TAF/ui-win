using System;
using System.Runtime.InteropServices;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    internal enum KeyUpDown
    {
        KEYEVENTF_KEYDOWN = 0x0000,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardInput
    {
        private readonly short vk;
        private readonly short scan;
        private readonly KeyUpDown flags;
        private readonly int time;
        private readonly IntPtr extraInfo;

        public KeyboardInput(short wVk, KeyUpDown flags, IntPtr extraInfo)
        {
            this.vk = wVk;
            this.scan = 0;
            this.flags = flags;
            this.time = 0;
            this.extraInfo = extraInfo;
        }
    }
}
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
