using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base tooltip control.
    /// </summary>
    public class ToolTip : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTip"/> class.
        /// </summary>
        public ToolTip()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolTip"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public ToolTip(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA tooltip control type.
        /// </summary>
        public override ControlType UiaType => ControlType.ToolTip;
    }
}
