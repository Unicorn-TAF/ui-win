using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base button control.
    /// </summary>
    public class Button : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Button(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA button control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ButtonControlTypeId;
    }
}
