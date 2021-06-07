using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base progress bar control.
    /// </summary>
    public class ProgressBar : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class.
        /// </summary>
        public ProgressBar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressBar"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public ProgressBar(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA progress bar control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ProgressBarControlTypeId;

        /// <summary>
        /// Gets current progress percentage as <see cref="double"/>
        /// </summary>
        public double CurrentProgress => ValuePattern.CurrentValue;

        /// <summary>
        /// Gets value pattern instance.
        /// </summary>
        protected IUIAutomationRangeValuePattern ValuePattern =>
            Instance.GetPattern<IUIAutomationRangeValuePattern>();
    }
}
