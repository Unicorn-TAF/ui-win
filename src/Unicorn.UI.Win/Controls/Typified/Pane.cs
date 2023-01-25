using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base pane control.
    /// </summary>
    public class Pane : WinContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Pane"/> class.
        /// </summary>
        public Pane()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pane"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Pane(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA pane control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_PaneControlTypeId;
    }
}
