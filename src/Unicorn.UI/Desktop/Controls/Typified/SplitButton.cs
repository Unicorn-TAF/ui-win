using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base split-button control.
    /// </summary>
    public class SplitButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SplitButton"/> class.
        /// </summary>
        public SplitButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SplitButton"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public SplitButton(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA split-button control type.
        /// </summary>
        public override ControlType UiaType => ControlType.SplitButton;
    }
}