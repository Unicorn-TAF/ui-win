using System.Windows.Automation;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class ListItem : GuiControl, ISelectable
    {
        public ListItem()
        {
        }

        public ListItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.ListItem;

        public bool Selected => GetPattern<SelectionItemPattern>().Current.IsSelected;

        public bool Select()
        {
            if (this.Selected)
            {
                return false;
            }

            var pattern = GetPattern<SelectionItemPattern>();
            if (pattern != null)
            {
                pattern.Select();
            }
            else
            {
                this.Click();
            }

            return true;
        }
    }
}
