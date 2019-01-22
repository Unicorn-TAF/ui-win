using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Unicorn.UI.Core.Input
{
    /// <summary>
    /// Intended for White Internal use only
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
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

    /// <summary>
    /// Intended for White Internal use only
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
        private readonly short vk;
        private readonly short scan;
        private readonly KeyUpDown flags;
        private readonly int time;
        private readonly IntPtr extraInfo;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables

        public KeyboardInput(short wVk, KeyUpDown flags, IntPtr extraInfo)
        {
            this.vk = wVk;
            this.scan = 0;
            this.flags = flags;
            this.time = 0;
            this.extraInfo = extraInfo;
        }

        public enum KeyUpDown
        {
            KEYEVENTF_KEYDOWN = 0x0000,
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
        }

        public enum SpecialKeys
        {
            // http://pinvoke.net/default.aspx/user32/SendInput.html, http://delphi.about.com/od/objectpascalide/l/blvkc.htm
            SHIFT = 0x10,
            CONTROL = 0x11,
            ALT = 0x12,
            LEFT_ALT = 0xA4,
            RIGHT_ALT = 0xA5,
            RETURN = 0x0D,
            RIGHT = 0x27,
            BACKSPACE = 0x08,
            LEFT = 0x25,
            ESCAPE = 0x1B,
            TAB = 0x09,
            HOME = 0x24,
            END = 0x23,
            UP = 0x26,
            DOWN = 0x28,
            INSERT = 0x2D,
            DELETE = 0x2E,
            CAPS = 0x14,
            ENTER = 0x0D,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            PAGEUP = 0x21,
            PAGEDOWN = 0x22,
            PRINT = 0x2A,
            PRINTSCREEN = 0x2C,
            SPACE = 0x20,
            NUMLOCK = 0x90,
            SCROLL = 0x91,
            LWIN = 0x5B,
            RWIN = 0x5C,
            MENU = 0x5D,
        }
    }

#pragma warning disable S1144 // Unused private types or members should be removed
    /// <summary>
    /// Intended for White Internal use only
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        private int msg;
        private short paramL;
        private short paramH;
    }
#pragma warning restore S1144 // Unused private types or members should be removed

    /// <summary>
    /// Intended for White Internal use only
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CursorInfo
    {
        public uint Size;
        public uint Flags;
        public Point Point;
#pragma warning disable S1144 // Unused private types or members should be removed
        private IntPtr handle;
#pragma warning restore S1144 // Unused private types or members should be removed

        public static CursorInfo New()
        {
            CursorInfo info = new CursorInfo();
            info.Size = (uint)Marshal.SizeOf(typeof(CursorInfo));
            return info;
        }
    }

    public static class WindowsConstants
    {
        public const uint SwHide = 0;
        public const uint SwShowNormal = 1;
        public const uint SwNormal = 1;
        public const uint SwShowMinimized = 2;
        public const uint SwShowMaximized = 3;
        public const uint SwMaximize = 3;
        public const uint SwShowNoActivate = 4;
        public const uint SwShow = 5;
        public const uint SwMinimize = 6;
        public const uint SwShowMinNoActive = 7;
        public const uint SwShowNa = 8;
        public const uint SwRestore = 9;
        public const uint SwShowDefault = 10;
        public const uint SwForceMinimize = 11;
        public const uint SwMax = 11;

        public const long WsCaption = 0x00C00000L;
        public const long WsDisabled = 0x08000000L;
        public const long WsVScroll = 0x00200000L;
        public const long WsHScroll = 0x00100000L;
        public const long WsMinimizeBox = 0x00020000L;
        public const long WsMaximizeBox = 0x00010000L;
        public const long WsPopup = 0x80000000L;
        public const long WsSysmenu = 0x00080000L;
        public const long WsTabstop = 0x00010000L;
        public const long WsVisible = 0x10000000L;

        public const int InputMouse = 0;
        public const int InputKeyboard = 1;

        public const int MouseEventFMove = 0x0001;
        public const int MouseEventFLeftDown = 0x0002;
        public const int MouseEventFLeftUp = 0x0004;
        public const int MouseEventFRightDown = 0x0008;
        public const int MouseEventFRightUp = 0x0010;
        public const int MouseEventFMiddleDown = 0x0020;
        public const int MouseEventFMiddleUp = 0x0040;
        public const int MouseEventFXDown = 0x0080;
        public const int MouseEventFXUp = 0x0100;
        public const int MouseEventFWheel = 0x0800;
        public const int MouseEventFVirtualDesk = 0x4000;
        public const int MouseEventFAbsolute = 0x8000;

        public const int HourGlassValue = 65557;

        public static uint WpfRestoreToMaximized => 2;
    }
}
