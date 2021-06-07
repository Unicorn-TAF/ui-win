using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base status bar control.
    /// </summary>
    public class StatusBar : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public StatusBar(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA status bar control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_StatusBarControlTypeId;
    }
}
