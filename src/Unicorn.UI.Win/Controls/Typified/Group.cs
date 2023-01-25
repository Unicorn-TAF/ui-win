using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base group control.
    /// </summary>
    public class Group : WinContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Group(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA group control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_GroupControlTypeId;
    }
}
