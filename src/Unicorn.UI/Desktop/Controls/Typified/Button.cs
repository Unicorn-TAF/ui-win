using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base button control.
    /// </summary>
    public class Button : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        public Button()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Button(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA button control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Button;
    }
}
