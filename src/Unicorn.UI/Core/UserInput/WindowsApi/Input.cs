using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Core.UserInput.WindowsApi
{
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "mouseInput", Justification = "Skip for now")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "keyboardInput", Justification = "Skip for now")]
    [SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", MessageId = "extraInfo", Justification = "Skip for now")]
    [StructLayout(LayoutKind.Explicit)]
    internal struct Input
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Skip for now")]
        [FieldOffset(0)]
        private int type;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Skip for now")]
        [FieldOffset(4)]
        private MouseInput mouseInput;

        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Skip for now")]
        [FieldOffset(4)]
        private KeyboardInput keyboardInput;

        internal static Input Mouse(MouseInput input) =>
            new Input
            {
                type = Constants.InputMouse,
                mouseInput = input
            };

        internal static Input Keyboard(KeyboardInput input) =>
            new Input
            {
                type = Constants.InputKeyboard,
                keyboardInput = input
            };
    }
}
