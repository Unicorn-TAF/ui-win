using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Group : GuiContainer
    {
        public Group() { }

        public Group(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType UiaType => ControlType.Group;
    }
}
