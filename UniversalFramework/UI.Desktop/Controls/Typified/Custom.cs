using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Custom : GuiControl
    {
        public Custom()
        {
        }

        public Custom(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.Custom;
    }
}
