using UIAutomationClient;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class ListView : WinContainer
    {
        public ListView()
        {
        }

        public ListView(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int UiaType => UIA_ControlTypeIds.UIA_ListControlTypeId;

        public virtual bool SelectItem(string itemName) =>
            this.Find<TreeItem>(ByLocator.Name(itemName)).Select();
    }
}
