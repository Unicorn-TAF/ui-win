using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base status bar control.
    /// </summary>
    public class StatusBar : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class.
        /// </summary>
        public StatusBar()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusBar"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public StatusBar(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA status bar control type.
        /// </summary>
        public override ControlType UiaType => ControlType.StatusBar;
    }
}
