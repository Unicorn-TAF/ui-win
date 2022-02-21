using System.Runtime.InteropServices;

namespace Unicorn.UI.Win.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        internal string dmDeviceName;
        internal short dmSpecVersion;
        internal short dmDriverVersion;
        internal short dmSize;
        internal short dmDriverExtra;
        internal int dmFields;
        internal int dmPositionX;
        internal int dmPositionY;
        internal int dmDisplayOrientation;
        internal int dmDisplayFixedOutput;
        internal short dmColor;
        internal short dmDuplex;
        internal short dmYResolution;
        internal short dmTTOption;
        internal short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
        internal string dmFormName;
        internal short dmLogPixels;
        internal int dmBitsPerPel;
        internal int dmPelsWidth;
        internal int dmPelsHeight;
        internal int dmDisplayFlags;
        internal int dmDisplayFrequency;
        internal int dmICMMethod;
        internal int dmICMIntent;
        internal int dmMediaType;
        internal int dmDitherType;
        internal int dmReserved1;
        internal int dmReserved2;
        internal int dmPanningWidth;
        internal int dmPanningHeight;
    }
}
