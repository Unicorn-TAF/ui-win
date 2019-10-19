using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base text control.
    /// </summary>
    public class Text : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        public Text()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Text(AutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA text control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Text;
    }
}
