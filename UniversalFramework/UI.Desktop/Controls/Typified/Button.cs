using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Button : GuiControl
    {
        public Button()
        {
        }

        public Button(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.Button;
    }
}
