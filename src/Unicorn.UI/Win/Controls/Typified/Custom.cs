using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Custom : WinContainer
    {
        public Custom()
        {
        }

        public Custom(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_CustomControlTypeId;
    }
}
