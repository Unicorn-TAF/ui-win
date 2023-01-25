using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base text control.
    /// </summary>
    public class Text : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        public Text()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Text(IUIAutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA text control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_TextControlTypeId;
    }
}
