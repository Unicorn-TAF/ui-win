using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base custom control.
    /// </summary>
    public class Custom : WinContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Custom"/> class.
        /// </summary>
        public Custom()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Custom"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Custom(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA custom control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_CustomControlTypeId;
    }
}
