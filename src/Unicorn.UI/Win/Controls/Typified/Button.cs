using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Button : WinControl
    {
        public Button()
        {
        }

        public Button(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ButtonControlTypeId;
    }
}
