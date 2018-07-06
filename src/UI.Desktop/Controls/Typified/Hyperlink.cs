using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Hyperlink : GuiControl
    {
        public Hyperlink() { }

        public Hyperlink(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.Hyperlink;
    }
}
