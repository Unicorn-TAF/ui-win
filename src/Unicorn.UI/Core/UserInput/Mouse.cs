using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Unicorn.UI.Core.UserInput.WindowsApi;

namespace Unicorn.UI.Core.UserInput
{
    /// <summary>
    /// Mouse.
    /// </summary>
    public class Mouse
    {
        private const int ExtraMillisecondsBecauseOfBugInWindows = 13;
        private static Mouse _instance = null;
        private readonly short _doubleClickTime = GetDoubleClickTime();
        private DateTime _lastClickTime = DateTime.Now;
        private Point _lastClickLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mouse"/> class.
        /// </summary>
        protected Mouse()
        {
        }

        /// <summary>
        /// Gets mouse instance.
        /// </summary>
        public static Mouse Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Mouse();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets or sets mouse current location as <see cref="Point"/>.
        /// </summary>
        public Point Location
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
                    throw new InvalidOperationException($"Trying to set location outside the screen. {value}");
                }

                SetCursorPos(new System.Drawing.Point((int)value.X, (int)value.Y));
            }
        }

        /// <summary>
        /// Performs right mouse click on current pointer location.
        /// </summary>
        public void RightClick()
        {
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFRightDown, GetMessageExtraInfo())));
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFRightUp, GetMessageExtraInfo())));
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
            if (_lastClickLocation.Equals(clickLocation))
            {
                int timeout = _doubleClickTime - DateTime.Now.Subtract(_lastClickTime).Milliseconds;
                if (timeout > 0)
                {
                    Thread.Sleep(timeout + ExtraMillisecondsBecauseOfBugInWindows);
                }
            }

            MouseLeftButtonUpAndDown();
            _lastClickTime = DateTime.Now;
            _lastClickLocation = Location;
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
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFLeftDown, GetMessageExtraInfo())));

        /// <summary>
        /// Performs left button up action on current mouse position.
        /// </summary>
        public void LeftButtonUp() =>
            SendInput(Input.Mouse(new MouseInput(Constants.MouseEventFLeftUp, GetMessageExtraInfo())));

        internal int SendInput(Input input) =>
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
            LeftButtonDown();
            LeftButtonUp();
        }

        private bool PointIsInvalid(Point p) =>
            double.IsNaN(p.X) || double.IsNaN(p.Y) ||
            p.X == double.PositiveInfinity || p.X == double.NegativeInfinity ||
            p.Y == double.PositiveInfinity || p.Y == double.NegativeInfinity;
    }
}
