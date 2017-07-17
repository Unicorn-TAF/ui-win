using System;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;


namespace Unicorn.UIDesktop.Input
{
    public class Mouse
    {
        [DllImport("user32", EntryPoint = "SendInput")]
        private static extern int SendInput(int numberOfInputs, ref Input input, int structSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        private static extern bool GetCursorInfo(ref CursorInfo cursorInfo);

        [DllImport("user32.dll")]
        private static extern short GetDoubleClickTime();

        public static Mouse Instance = new Mouse();
        private DateTime lastClickTime = DateTime.Now;
        private readonly short doubleClickTime = GetDoubleClickTime();
        private Point lastClickLocation;
        private const int extraMillisecondsBecauseOfBugInWindows = 13;

        private Mouse() { }

        public virtual Point Location
        {
            get
            {
                var point = new System.Drawing.Point();
                GetCursorPos(ref point);
                return point.ConvertToWindowsPoint();
            }
            set
            {
                if (value.IsInvalid())
                {
                    throw new Exception(string.Format("Trying to set location outside the screen. {0}", value));
                }
                SetCursorPos(value.ToDrawingPoint());
            }
        }

  
        public virtual void RightClick()
        {
            SendInput(Input.Mouse(MouseInput(WindowsConstants.MOUSEEVENTF_RIGHTDOWN)));
            SendInput(Input.Mouse(MouseInput(WindowsConstants.MOUSEEVENTF_RIGHTUP)));
        }

        public virtual void ResetPosition()
        {
            Instance.Location = new Point(0, 0);
        }

        public virtual void Click()
        {
            Point clickLocation = Location;
            if (lastClickLocation.Equals(clickLocation))
            {
                int timeout = doubleClickTime - DateTime.Now.Subtract(lastClickTime).Milliseconds;
                if (timeout > 0) Thread.Sleep(timeout + extraMillisecondsBecauseOfBugInWindows);
            }
            MouseLeftButtonUpAndDown();
            lastClickTime = DateTime.Now;
            lastClickLocation = Location;
        }

        public static void LeftUp()
        {
            SendInput(Input.Mouse(MouseInput(WindowsConstants.MOUSEEVENTF_LEFTUP)));
        }

        public static void LeftDown()
        {
            SendInput(Input.Mouse(MouseInput(WindowsConstants.MOUSEEVENTF_LEFTDOWN)));
        }

        public virtual void DoubleClick(Point point)
        {
            Location = point;
            MouseLeftButtonUpAndDown();
            MouseLeftButtonUpAndDown();
        }

        private static void SendInput(Input input)
        {
            SendInput(1, ref input, Marshal.SizeOf(typeof(Input)));
        }

        private static MouseInput MouseInput(int command)
        {
            return new MouseInput(command, GetMessageExtraInfo());
        }

        public virtual void RightClick(Point point)
        {
            Location = point;
            RightClick();
        }

        public virtual void Click(Point point)
        {
            Location = point;
            Click();
        }

  

        public static void MouseLeftButtonUpAndDown()
        {
            LeftDown();
            LeftUp();
        }

        public virtual void MoveOut()
        {
            Location = new Point(0, 0);
        }


    }

    public static class DrawingPointX
    {
        public static System.Windows.Point ConvertToWindowsPoint(this System.Drawing.Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
    }

    public static class WindowsPointX
    {
        public static System.Drawing.Point ToDrawingPoint(this System.Windows.Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }

        public static bool IsInvalid(this System.Windows.Point point)
        {
            return point.X.IsInvalid() || point.Y.IsInvalid();
        }
    }

    public static class DoubleX
    {
        public static bool IsInvalid(this double @double)
        {
            return @double == double.PositiveInfinity || @double == double.NegativeInfinity || double.IsNaN(@double);
        }
    }
}
