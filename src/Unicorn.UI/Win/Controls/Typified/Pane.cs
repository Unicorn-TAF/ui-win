using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Pane : WinContainer
    {
        public Pane()
        {
        }

        public Pane(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_PaneControlTypeId;
    }
}
