using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Unicorn.Core.Logging;
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

        public bool Select(string item)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{item}' item from {this.ToString()}");

            if (item.Equals(this.SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Debug, "\tNo need to select (the item is selected by default)");
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

            Logger.Instance.Log(LogLevel.Debug, "\tItem was selected");

            return true;
        }

        public bool Expand()
        {
            Logger.Instance.Log(LogLevel.Debug, "\tExpanding dropdown");
            if (this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Debug, "\t\tNo need to expand (expanded by default)");
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Expand();
            Logger.Instance.Log(LogLevel.Debug, "\t\tExpanded");
            return true;
        }

        public bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Debug, "\tCollapsing dropdown");
            if (!this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Debug, "\t\tNo need to collapse (collapsed by default)");
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Collapse();
            Logger.Instance.Log(LogLevel.Debug, "\t\tCollapsed");
            return true;
        }
    }
}
