using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Text : WinControl
    {
        public Text()
        {
        }

        public Text(IUIAutomationElement instance) : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_TextControlTypeId;

    }
}
