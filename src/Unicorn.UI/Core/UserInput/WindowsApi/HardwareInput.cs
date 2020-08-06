using System.Runtime.InteropServices;

#pragma warning disable S1144 // Unused private types or members should be removed
namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HardwareInput
    {
        private readonly int _msg;
        private readonly short _paramL;
        private readonly short _paramH;
    }
}
#pragma warning restore S1144 // Unused private types or members should be removed
