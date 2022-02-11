using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Unicorn.UI.Win.WindowsApi;

namespace Unicorn.UI.Win.UserInput
{
    /// <summary>
    /// Mouse.
    /// </summary>
    public class Mouse
    {
        private const int ExtraMillisecondsBecauseOfBugInWindows = 13;
        private readonly short _doubleClickTime = NativeMethods.GetDoubleClickTime();
        private DateTime lastClickTime = DateTime.Now;
        private Point lastClickLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        private Mouse()
        {
        }

        /// <summary>
        /// Gets mouse instance.
        /// </summary>
        public static Mouse Instance = new Mouse();

        /// <summary>
        /// Gets or sets mouse current location as <see cref="Point"/>.
        /// </summary>
        public Point Location
        {
            get
            {
                var point = new Point();
                NativeMethods.GetCursorPos(ref point);
                return point;
            }

            set
            {
                if (PointIsInvalid(value))
                {
                    throw new InvalidOperationException($"Trying to set location outside the screen. {value}");
                }

                NativeMethods.SetCursorPos(value.X, value.Y);
            }
        }

        /// <summary>
        /// Performs right mouse click on current pointer location.
        /// </summary>
        public void RightClick()
        {
            SendInput(INPUT.Mouse(MouseFlag.MOUSEEVENTF_RIGHTDOWN));
            SendInput(INPUT.Mouse(MouseFlag.MOUSEEVENTF_RIGHTUP));
        }

        /// <summary>
        /// Moves mouse pointer to left upper screen corner.
        /// </summary>
        public void ResetPosition() =>
            Location = new Point(0, 0);

        /// <summary>
        /// Performs left mouse click on current pointer location.
        /// </summary>
        public void Click()
        {
            Point clickLocation = Location;
            if (lastClickLocation.Equals(clickLocation))
            {
                int timeout = _doubleClickTime - DateTime.Now.Subtract(lastClickTime).Milliseconds;
                if (timeout > 0)
                {
                    Thread.Sleep(timeout + ExtraMillisecondsBecauseOfBugInWindows);
                }
            }

            MouseLeftButtonUpAndDown();
            lastClickTime = DateTime.Now;
            lastClickLocation = Location;
        }

        /// <summary>
        /// Performs right mouse click on specified pointer location.
        /// </summary>
        /// <param name="point">point on screen to right click on</param>
        public void RightClick(Point point)
        {
            Location = point;
            RightClick();
        }

        /// <summary>
        /// Performs left mouse click on specified pointer location.
        /// </summary>
        /// <param name="point">point on screen to left click on</param>
        public void Click(Point point)
        {
            Location = point;
            Click();
        }

        /// <summary>
        /// Performs double left mouse click on specified pointer location.
        /// </summary>
        /// <param name="point">point on screen to left click on</param>
        public void DoubleClick(Point point)
        {
            Location = point;
            MouseLeftButtonUpAndDown();
            MouseLeftButtonUpAndDown();
        }

        /// <summary>
        /// Performs left button down action on current mouse position.
        /// </summary>
        public void LeftButtonDown() =>
            SendInput(INPUT.Mouse(MouseFlag.MOUSEEVENTF_LEFTDOWN));

        /// <summary>
        /// Performs left button up action on current mouse position.
        /// </summary>
        public void LeftButtonUp() =>
            SendInput(INPUT.Mouse(MouseFlag.MOUSEEVENTF_LEFTUP));

        private int SendInput(INPUT input) =>
            NativeMethods.SendInput(1, ref input, Marshal.SizeOf(typeof(INPUT)));

        private void MouseLeftButtonUpAndDown()
        {
            LeftButtonDown();
            LeftButtonUp();
        }

        private bool PointIsInvalid(Point p) =>
            double.IsNaN(p.X) || double.IsNaN(p.Y) ||
            double.IsInfinity(p.X) || double.IsInfinity(p.Y);
    }
}
