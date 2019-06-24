using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Unicorn.UI.Core.UserInput.WindowsApi;

namespace Unicorn.UI.Core.UserInput
{
    public class Mouse
    {
        private const int ExtraMillisecondsBecauseOfBugInWindows = 13;
        private static Mouse instance = null;
        private readonly short doubleClickTime = GetDoubleClickTime();
        private DateTime lastClickTime = DateTime.Now;
        private Point lastClickLocation;

        protected Mouse()
        {
        }

        public static Mouse Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Mouse();
                }

                return instance;
            }
        }

        public virtual Point Location
        {
            get
            {
                var point = new System.Drawing.Point();
                GetCursorPos(ref point);
                return new Point(point.X, point.Y);
            }

            set
            {
                if (PointIsInvalid(value))
                {
                    throw new InvalidOperationException(string.Format("Trying to set location outside the screen. {0}", value));
                }

                SetCursorPos(new System.Drawing.Point((int)value.X, (int)value.Y));
            }
        }

        public virtual void RightClick()
        {
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFRightDown, GetMessageExtraInfo())));
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFRightUp, GetMessageExtraInfo())));
        }

        public virtual void ResetPosition() =>
            Instance.Location = new Point(0, 0);

        public virtual void Click()
        {
            Point clickLocation = this.Location;
            if (this.lastClickLocation.Equals(clickLocation))
            {
                int timeout = this.doubleClickTime - DateTime.Now.Subtract(this.lastClickTime).Milliseconds;
                if (timeout > 0)
                {
                    Thread.Sleep(timeout + ExtraMillisecondsBecauseOfBugInWindows);
                }
            }

            MouseLeftButtonUpAndDown();
            this.lastClickTime = DateTime.Now;
            this.lastClickLocation = this.Location;
        }

        public virtual void RightClick(Point point)
        {
            this.Location = point;
            RightClick();
        }

        public virtual void Click(Point point)
        {
            this.Location = point;
            Click();
        }

        public virtual void MoveOut() => this.Location = new Point(0, 0);

        public virtual void DoubleClick(Point point)
        {
            this.Location = point;
            MouseLeftButtonUpAndDown();
            MouseLeftButtonUpAndDown();
        }

        private static int SendInput(Input input) =>
            SendInput(1, ref input, Marshal.SizeOf(typeof(Input)));

        [DllImport("user32", EntryPoint = "SendInput")]
        private static extern int SendInput(int numberOfInputs, ref Input input, int structSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(ref System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(System.Drawing.Point cursorInfo);

        [DllImport("user32.dll")]
        private static extern short GetDoubleClickTime();

        private void MouseLeftButtonUpAndDown()
        {
            LeftDown();
            LeftUp();
        }

        private void LeftUp() =>
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFLeftUp, GetMessageExtraInfo())));

        private void LeftDown() =>
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFLeftDown, GetMessageExtraInfo())));

        private bool PointIsInvalid(Point p) =>
            double.IsNaN(p.X) || double.IsNaN(p.Y) ||
            p.X == double.PositiveInfinity || p.X == double.NegativeInfinity ||
            p.Y == double.PositiveInfinity || p.Y == double.NegativeInfinity;
    }
}
