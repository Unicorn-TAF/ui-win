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

        public override int UiaType => UIA_ControlTypeIds.UIA_HyperlinkControlTypeId;
    }
}
