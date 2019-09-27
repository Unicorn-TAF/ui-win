using UIAutomationClient;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class Image : WinControl
    {
        public Image()
        {
        }

        public Image(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ImageControlTypeId;
    }
}
