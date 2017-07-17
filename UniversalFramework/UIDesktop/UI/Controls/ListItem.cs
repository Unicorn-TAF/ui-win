using System.Windows.Automation;
using Unicorn.UICore.UI;

namespace Unicorn.UIDesktop.UI.Controls
{
    public class ListItem : GuiControl, ISelectable
    {
        public override ControlType Type { get { return ControlType.ListItem; } }

        public ListItem() { }

        public ListItem(AutomationElement instance)
            : base(instance)
        {
        }


        public bool Select()
        {
            if (IsSelected)
                return false;

            var pattern = GetPattern<SelectionItemPattern>();
            if (pattern != null)
                pattern.Select();
            else
                Click();

            return true;
        }


        public bool IsSelected
        {
            get { return (Instance.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Current.IsSelected; }
        }
    }
}
