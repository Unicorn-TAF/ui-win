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

        public override int Type => UIA_ControlTypeIds.UIA_ListControlTypeId;
    }
}
