using System;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Unicorn.UICore.UI.Controls;

namespace Unicorn.UIDesktop.UI.Controls
{
    public class Dropdown : GuiControl, IDropdown
    {
        public override ControlType Type { get { return ControlType.ComboBox; } }


        public Dropdown() { }

        public Dropdown(AutomationElement instance)
            : base(instance)
        {
        }

        public bool isExpanded
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SelectedValue
        {
            get
            {
                var selection = GetPattern<SelectionPattern>();
                if (selection != null)
                {
                    var item = selection.Current.GetSelection().FirstOrDefault();
                    if (item != null)
                        return GetAttribute("text");
                }
                var value = GetPattern<ValuePattern>();
                if (value != null)
                    return value.Current.Value;
                return "";
            }
        }
        

        public void CheckItems(string[] items)
        {
            throw new NotImplementedException();
        }

        public bool Select(string item)
        {
            if (item.Equals(SelectedValue))
                return false;

            var valuePattern = GetPattern<ValuePattern>();
            if (valuePattern != null)
                valuePattern.SetValue(item);
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(item);
                if (itemEl != null)
                    itemEl.Select();
                Collapse();
                Thread.Sleep(500);
            }
            return true;
        }

        public void Expand()
        {
            GetPattern<ExpandCollapsePattern>().Expand();
        }

        public void Collapse()
        {
            GetPattern<ExpandCollapsePattern>().Collapse();
        }
    }
}
