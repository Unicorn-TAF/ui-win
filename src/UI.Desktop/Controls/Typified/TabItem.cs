using System;
using System.Windows.Automation;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class TabItem : GuiControl
    {
        public TabItem()
        {
        }

        public TabItem(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.TabItem;

        public bool IsSelected
        {
            get
            {
                var selectionItem = GetPattern<SelectionItemPattern>();
                if (selectionItem != null)
                {
                    return selectionItem.Current.IsSelected;
                }

                return false;
            }
        }

        public bool Select()
        {
            var selectionItem = GetPattern<SelectionItemPattern>();
            if (selectionItem != null)
            {
                selectionItem.Select();
            }
            else
            {
                var invoke = GetPattern<InvokePattern>();
                if (invoke != null)
                {
                    invoke.Invoke();
                }
                else
                {
                    this.Click();
                }
            }

            return true;
        }
    }
}