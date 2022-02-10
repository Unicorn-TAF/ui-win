using System;
using System.Runtime.InteropServices;

#pragma warning disable S1144 // Unused private types or members should be removed
namespace Unicorn.UI.Win.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseInput
    {
        private readonly int _dx;
        private readonly int _dy;
        private readonly int _mouseData;
        private readonly int _flags;
        private readonly int _time;
        private readonly IntPtr _extraInfo;

        internal MouseInput(int flags, IntPtr extraInfo)
        {
            _flags = flags;
            _extraInfo = extraInfo;
            _dx = 0;
            _dy = 0;
            _time = 0;
            _mouseData = 0;
        }
    }
}
#pragma warning restore S1144 // Unused private types or members should be removed
