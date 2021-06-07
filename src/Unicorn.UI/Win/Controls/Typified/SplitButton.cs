using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base split-button control.
    /// </summary>
    public class SplitButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplitButton"/> class.
        /// </summary>
        public SplitButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitButton"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public SplitButton(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA split-button control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_SplitButtonControlTypeId;
    }
}
