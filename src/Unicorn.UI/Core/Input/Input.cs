using System.Runtime.InteropServices;

namespace Unicorn.UI.Core.Input
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        [FieldOffset(4)]
        private readonly HardwareInput hi;

        [FieldOffset(0)]
        private int type;
        [FieldOffset(4)]
        private MouseInput mi;
        [FieldOffset(4)]
        private KeyboardInput ki;

        public static Input Mouse(MouseInput mouseInput) =>
            new Input { type = WindowsConstants.InputMouse, mi = mouseInput };

        public static Input Keyboard(KeyboardInput keyboardInput) =>
            new Input { type = WindowsConstants.InputKeyboard, ki = keyboardInput };
    }
}
