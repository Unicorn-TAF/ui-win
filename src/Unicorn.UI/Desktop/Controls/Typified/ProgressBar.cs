using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base progress bar control.
    /// </summary>
    public class ProgressBar : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        public ProgressBar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public ProgressBar(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA progress bar control type.
        /// </summary>
        public override ControlType UiaType => ControlType.ProgressBar;

        /// <summary>
        /// Gets current progress percentage as <see cref="double"/>
        /// </summary>
        public double CurrentProgress => Instance.GetPattern<RangeValuePattern>().Current.Value;
    }
}
