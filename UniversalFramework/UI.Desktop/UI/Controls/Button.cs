using System.Windows.Automation;

namespace Unicorn.UI.Desktop.UI.Controls
{
    public class Button : GuiControl
    {
        public Button() { }

        public Button(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type { get { return ControlType.Button; } }
    }
}
