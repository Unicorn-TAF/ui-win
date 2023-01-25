using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base tooltip control.
    /// </summary>
    public class ToolTip : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTip"/> class.
        /// </summary>
        public ToolTip()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTip"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public ToolTip(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tooltip control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ToolTipControlTypeId;
    }
}
