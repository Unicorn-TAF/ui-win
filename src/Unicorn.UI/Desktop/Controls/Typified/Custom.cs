using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base custom control.
    /// </summary>
    public class Custom : GuiContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Custom"/> class.
        /// </summary>
        public Custom()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Custom"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Custom(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA custom control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Custom;
    }
}
