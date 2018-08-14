using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Custom : WinControl
    {
        public Custom()
        {
        }

        public Custom(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_CustomControlTypeId;
    }
}
