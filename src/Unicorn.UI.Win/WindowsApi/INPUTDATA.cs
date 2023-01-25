using System.Runtime.InteropServices;

namespace Unicorn.UI.Win.WindowsApi
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct INPUTDATA
    {
        [FieldOffset(0)]
        public MOUSEINPUT Mi;

        [FieldOffset(0)]
        public KEYBDINPUT Ki;

        [FieldOffset(0)]
        public HARDWAREINPUT Hi;
    }
}
