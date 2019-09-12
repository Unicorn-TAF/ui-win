namespace Unicorn.UI.Core.UserInput
{
    internal static class Constants
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

        public const uint WpfRestoreToMaximized = 2;
    }
}
