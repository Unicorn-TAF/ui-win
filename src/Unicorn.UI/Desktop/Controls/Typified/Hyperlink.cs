using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base hyperlink control.
    /// </summary>
    public class Hyperlink : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperlink"/> class.
        /// </summary>
        public Hyperlink()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hyperlink"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Hyperlink(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA hyperlink control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Hyperlink;
    }
}
