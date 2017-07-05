using System;
using System.Windows.Automation;
using Unicorn.UICore.UI;

namespace Unicorn.UIDesktop.UI.Controls
{
    class TabItem : GuiControl, ISelectable
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