using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Custom : GuiContainer
    {
        public Custom() { }

        public Custom(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Custom;
    }
}
