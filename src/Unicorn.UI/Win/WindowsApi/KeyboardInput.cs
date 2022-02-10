using System;
using System.Runtime.InteropServices;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
namespace Unicorn.UI.Win.WindowsApi
{
    internal class KeyUpDown
    {
        internal const uint KEYEVENTF_KEYDOWN = 0x0000;
        internal const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        internal const uint KEYEVENTF_KEYUP = 0x0002;
        internal const uint Scancode = 0x0008;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KeyboardInput
    {
        private readonly ushort _vk;
        private readonly ushort _scan;
        private readonly uint _flags;
        private readonly uint _time;
        private readonly IntPtr _extraInfo;

        internal KeyboardInput(ushort wVk, uint flags, IntPtr extraInfo)
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
