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
        private readonly short _vk;
        private readonly short _scan;
        private readonly KeyUpDown _flags;
        private readonly int _time;
        private readonly IntPtr _extraInfo;

        internal KeyboardInput(short wVk, KeyUpDown flags, IntPtr extraInfo)
        {
            _vk = wVk;
            _scan = 0;
            _flags = flags;
            _time = 0;
            _extraInfo = extraInfo;
        }
    }
}
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
