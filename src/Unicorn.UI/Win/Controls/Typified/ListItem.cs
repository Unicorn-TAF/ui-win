using UIAutomationClient;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Win.Controls.Typified
{
    public class ListItem : WinControl, ISelectable
    {
        public ListItem()
        {
        }

        public ListItem(IUIAutomationElement instance)
            : base(instance)
        {
        }

        public override int Type => UIA_ControlTypeIds.UIA_ListItemControlTypeId;

        protected IUIAutomationSelectionItemPattern SelectionItemPattern => base.GetPattern(UIA_PatternIds.UIA_SelectionItemPatternId) as IUIAutomationSelectionItemPattern;

        public bool Selected => this.SelectionItemPattern.CurrentIsSelected != 0;

        public bool Select()
        {
            if (this.Selected)
            {
                return false;
            }

            var pattern = this.SelectionItemPattern;

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
