using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class HeaderItem : GuiControl
    {
        public HeaderItem()
        {
        }

        public HeaderItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.HeaderItem;
    }
}
