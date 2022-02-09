using System.Drawing;
using System.Threading;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.UserInput;

namespace Unicorn.UI.Win
{
    /// <summary>
    /// Common actions under controls
    /// </summary>
    public class UiActions
    {
        /// <summary>
        /// Performs mouse drag and drop operation
        /// </summary>
        /// <param name="sourceControl">control to drag</param>
        /// <param name="targetControl">control to drop to</param>
        public void DragAndDrop(WinControl sourceControl, WinControl targetControl)
        {
            Mouse.Instance.Location = CenterOf(sourceControl);
            Mouse.Instance.LeftButtonDown();
            Thread.Sleep(50);
            Mouse.Instance.Location = CenterOf(targetControl);
            Mouse.Instance.LeftButtonUp();
        }

        /// <summary>
        /// Selects multiple controls by mouse 
        /// using <see cref="Keyboard.SpecialKeys.Control"/> key
        /// </summary>
        /// <param name="controls">controls to select</param>
        public void SelectMultipleControls(params WinControl[] controls)
        {
            Keyboard.Instance.HoldKey(Keyboard.SpecialKeys.Control);

            foreach (var control in controls)
            {
                control.MouseClick();
            }

            Keyboard.Instance.LeaveAllKeys();
        }

        /// <summary>
        /// Selects range of controls between two specified controls 
        /// by mouse using <see cref="Keyboard.SpecialKeys.Shift"/> key
        /// </summary>
        /// <param name="startControl">range start control</param>
        /// <param name="endControl">range end control</param>
        public void SelectControlsRange(WinControl startControl, WinControl endControl)
        {
            Keyboard.Instance.HoldKey(Keyboard.SpecialKeys.Shift);

            startControl.MouseClick();
            endControl.MouseClick();

            Keyboard.Instance.LeaveAllKeys();
        }

        private Point CenterOf(WinControl control)
        {
            var rect = control.BoundingRectangle;
            var point = new Point(rect.Left, rect.Top);
            point.Offset(rect.Width / 2, rect.Height / 2);

            return point;
        }
    }
}
