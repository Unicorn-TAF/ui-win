namespace Unicorn.UI.Core.UserInput
{
    internal static class Constants
    {
        internal const uint SwHide = 0;
        internal const uint SwShowNormal = 1;
        internal const uint SwNormal = 1;
        internal const uint SwShowMinimized = 2;
        internal const uint SwShowMaximized = 3;
        internal const uint SwMaximize = 3;
        internal const uint SwShowNoActivate = 4;
        internal const uint SwShow = 5;
        internal const uint SwMinimize = 6;
        internal const uint SwShowMinNoActive = 7;
        internal const uint SwShowNa = 8;
        internal const uint SwRestore = 9;
        internal const uint SwShowDefault = 10;
        internal const uint SwForceMinimize = 11;
        internal const uint SwMax = 11;

        internal const long WsCaption = 0x00C00000L;
        internal const long WsDisabled = 0x08000000L;
        internal const long WsVScroll = 0x00200000L;
        internal const long WsHScroll = 0x00100000L;
        internal const long WsMinimizeBox = 0x00020000L;
        internal const long WsMaximizeBox = 0x00010000L;
        internal const long WsPopup = 0x80000000L;
        internal const long WsSysmenu = 0x00080000L;
        internal const long WsTabstop = 0x00010000L;
        internal const long WsVisible = 0x10000000L;

        internal const int InputMouse = 0;
        internal const int InputKeyboard = 1;

        internal const int MouseEventFMove = 0x0001;
        internal const int MouseEventFLeftDown = 0x0002;
        internal const int MouseEventFLeftUp = 0x0004;
        internal const int MouseEventFRightDown = 0x0008;
        internal const int MouseEventFRightUp = 0x0010;
        internal const int MouseEventFMiddleDown = 0x0020;
        internal const int MouseEventFMiddleUp = 0x0040;
        internal const int MouseEventFXDown = 0x0080;
        internal const int MouseEventFXUp = 0x0100;
        internal const int MouseEventFWheel = 0x0800;
        internal const int MouseEventFVirtualDesk = 0x4000;
        internal const int MouseEventFAbsolute = 0x8000;

        internal const int HourGlassValue = 65557;

        internal const uint WpfRestoreToMaximized = 2;
    }
}
