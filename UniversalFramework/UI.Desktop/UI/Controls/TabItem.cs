using System;
using System.Windows.Automation;
using Unicorn.UI.Core.UI;

namespace Unicorn.UI.Desktop.UI.Controls
{
    class TabItem : GuiControl
    {
        public override ControlType Type { get { return ControlType.TabItem; } }

        public TabItem() { }

        public TabItem(AutomationElement instance)
            : base(instance)
        {
        }


        public bool IsSelected
        {
            get
            {
                var selectionItem = GetPattern<SelectionItemPattern>();
                if (selectionItem != null)
                    return selectionItem.Current.IsSelected;
                return false;
            }
        }

        public bool Select()
        {
            var selectionItem = GetPattern<SelectionItemPattern>();
            if (selectionItem != null)
                selectionItem.Select();
            else
            {
                var invoke = GetPattern<InvokePattern>();
                if (invoke != null)
                    invoke.Invoke();
                else
                {
                    Click();
                }
            }
            return true;
        }
    }
}