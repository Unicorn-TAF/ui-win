using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class ListItem : GuiControl
    {
        public ListItem()
        {
        }

        public ListItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.ListItem;

        public bool IsSelected
        {
            get { return (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected; }
        }

        public bool Select()
        {
            if (IsSelected)
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
