using System;
using System.Runtime.InteropServices;

#pragma warning disable S1144 // Unused private types or members should be removed
namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct MouseInput
    {
        private readonly int dx;
        private readonly int dy;
        private readonly int mouseData;
        private readonly int flags;
        private readonly int time;
        private readonly IntPtr extraInfo;

        public MouseInput(int flags, IntPtr extraInfo)
        {
            this.flags = flags;
            this.extraInfo = extraInfo;
            this.dx = 0;
            this.dy = 0;
            this.time = 0;
            this.mouseData = 0;
        }
    }
}
#pragma warning restore S1144 // Unused private types or members should be removed
