using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class ListView : GuiControl
    {
        public ListView()
        {
        }

        public ListView(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.List;
    }
}
