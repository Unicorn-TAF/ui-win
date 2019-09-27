using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Pane : GuiContainer
    {
        public Pane()
        {
        }

        public Pane(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Pane;
    }
}
