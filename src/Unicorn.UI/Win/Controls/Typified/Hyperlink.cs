using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Hyperlink : WinControl
    {
        public Hyperlink()
        {
        }

        public Hyperlink(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_HyperlinkControlTypeId;
    }
}
