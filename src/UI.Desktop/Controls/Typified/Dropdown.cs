using System;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    public class Dropdown : GuiControl, IDropdown
    {
        public Dropdown()
        {
        }

        public Dropdown(AutomationElement instance)
            : base(instance)
        {
        }

        public override ControlType Type => ControlType.ComboBox;

        public bool Expanded
        {
            get
            {
                return GetPattern<ExpandCollapsePattern>().Current.ExpandCollapseState == ExpandCollapseState.Expanded;
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
                    {
                        return GetAttribute("text");
                    }
                }

                var value = GetPattern<ValuePattern>();

                if (value != null)
                {
                    return value.Current.Value;
                }
                    
                return string.Empty;
            }
        }

        public bool MultiSelect
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Select(string item)
        {
            if (item.Equals(this.SelectedValue))
            {
                return false;
            }

            var valuePattern = GetPattern<ValuePattern>();

            if (valuePattern != null)
            {
                valuePattern.SetValue(item);
            }
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(ByLocator.Name(item));

                if (itemEl != null)
                {
                    itemEl.Select();
                }
                    
                Collapse();
                Thread.Sleep(500);
            }

            return true;
        }

        public bool Expand()
        {
            if (this.Expanded)
            {
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Expand();
            return true;
        }

        public bool Collapse()
        {
            if (!this.Expanded)
            {
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Collapse();
            return true;
        }
    }
}
