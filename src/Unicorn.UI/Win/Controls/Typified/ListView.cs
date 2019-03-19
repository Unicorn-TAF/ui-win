using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class ListView : WinControl
    {
        public ListView()
        {
        }

        public ListView(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ListControlTypeId;
    }
}
