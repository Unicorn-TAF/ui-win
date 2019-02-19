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

        public bool Expanded => GetPattern<ExpandCollapsePattern>().Current.ExpandCollapseState.Equals(ExpandCollapseState.Expanded);

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
                        return item.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
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

        public bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {this.ToString()}");

            if (itemName.Equals(this.SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            var valuePattern = GetPattern<ValuePattern>();

            if (valuePattern != null)
            {
                valuePattern.SetValue(itemName);
            }
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(ByLocator.Name(itemName));

                if (itemEl != null)
                {
                    Logger.Instance.Log(LogLevel.Trace, "Item was found. Selecting...");
                    itemEl.Select();
                }
                    
                Collapse();
                Thread.Sleep(500);
            }

            Logger.Instance.Log(LogLevel.Trace, "Item was selected");

            return true;
        }

        public bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "Expanding dropdown");
            if (this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to expand (expanded by default)");
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Expand();
            Logger.Instance.Log(LogLevel.Trace, "Expanded");
            return true;
        }

        public bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "Collapsing dropdown");
            if (!this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to collapse (collapsed by default)");
                return false;
            }

            GetPattern<ExpandCollapsePattern>().Collapse();
            Logger.Instance.Log(LogLevel.Trace, "Collapsed");
            return true;
        }
    }
}
