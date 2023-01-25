using System;

namespace Unicorn.UI.Win.WindowsApi
{
    internal enum InputType : uint
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2,
    }

    internal struct INPUT
    {
        internal UInt32 Type;

        internal INPUTDATA Data;

        internal static INPUT Mouse(MouseFlag flag) =>
            new INPUT
            {
                Type = (UInt32)InputType.INPUT_MOUSE,
                Data = new INPUTDATA { Mi = new MOUSEINPUT((UInt32)flag) }
            };

        internal static INPUT Keyboard(UInt16 vk, UInt32 flag) =>
            new INPUT
            {
                Type = (UInt32)InputType.INPUT_KEYBOARD,
                Data = new INPUTDATA { Ki = new KEYBDINPUT(vk, flag) }
            };

        internal static INPUT Hardware(HARDWAREINPUT input) =>
            new INPUT
            {
                Type = (UInt32)InputType.INPUT_HARDWARE,
                Data = new INPUTDATA { Hi = input }
            };
    }
}
