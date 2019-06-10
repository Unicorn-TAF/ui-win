using System.Runtime.InteropServices;

#pragma warning disable S1144 // Unused private types or members should be removed
namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HardwareInput
    {
        private int msg;
        private short paramL;
        private short paramH;
    }
}
#pragma warning restore S1144 // Unused private types or members should be removed
