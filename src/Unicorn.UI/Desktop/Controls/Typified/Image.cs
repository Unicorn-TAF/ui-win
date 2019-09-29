using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base image control.
    /// </summary>
    public class Image : GuiControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Image(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA image control type.
        /// </summary>
        public override ControlType UiaType => ControlType.Image;
    }
}
