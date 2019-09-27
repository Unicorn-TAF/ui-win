using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Image : GuiControl
    {
        public Image()
        {
        }

        public Image(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Image;
    }
}
