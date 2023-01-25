using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base hyperlink control.
    /// </summary>
    public class Hyperlink : WinControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperlink"/> class.
        /// </summary>
        public Hyperlink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperlink"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Hyperlink(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA hyperlink control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_HyperlinkControlTypeId;
    }
}
