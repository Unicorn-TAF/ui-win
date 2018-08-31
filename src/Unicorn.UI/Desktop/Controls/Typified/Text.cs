using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Text : GuiControl
    {
        public Text()
        {
        }

        public Text(AutomationElement instance) : base(instance)
        {
        }

        public override ControlType Type => ControlType.Text;
    }
}
