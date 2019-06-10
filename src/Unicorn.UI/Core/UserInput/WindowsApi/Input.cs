using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "mouseInput")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "keyboardInput")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "hardwareInput")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "extraInfo")]
    [StructLayout(LayoutKind.Explicit)]
    internal struct Input
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        [FieldOffset(4)]
        private readonly HardwareInput hardwareInput;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        [FieldOffset(0)]
        private int type;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        [FieldOffset(4)]
        private MouseInput mouseInput;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        [FieldOffset(4)]
        private KeyboardInput keyboardInput;

        public static Input Mouse(MouseInput input) =>
            new Input
            {
                type = Constants.InputMouse,
                mouseInput = input
            };

        public static Input Keyboard(KeyboardInput input) =>
            new Input
            {
                type = Constants.InputKeyboard,
                keyboardInput = input
            };
    }
}
