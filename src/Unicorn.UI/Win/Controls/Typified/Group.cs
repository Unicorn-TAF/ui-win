using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Group : WinContainer
    {
        public Group()
        {
        }

        public Group(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_GroupControlTypeId;
    }
}
