using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base group control.
    /// </summary>
    public class Group : GuiContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Group(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA group control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Group;
    }
}
