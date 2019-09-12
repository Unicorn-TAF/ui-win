using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UnitTests.Gui.Desktop
{
    public class GuiDinamicDropdown : GuiControl, IDynamicDropdown
    {
        protected Dictionary<DropdownElement, ByLocator> locators = new Dictionary<DropdownElement, ByLocator>
        {
            { DropdownElement.TextInput, null },
            { DropdownElement.ExpandCollapse, null },
            { DropdownElement.List, null },
            { DropdownElement.ListItem, null },
        };

        public IControl ExpandCollapse => locators[DropdownElement.ExpandCollapse] == null ? 
            null : 
            this.Find<Button>(locators[DropdownElement.ExpandCollapse]);

        public ITextInput Input => locators[DropdownElement.TextInput] == null ?
            null :
            this.Find<TextInput>(locators[DropdownElement.TextInput]);

        public IControl ItemsContainer => locators[DropdownElement.List] == null ?
            null :
            this.Find<ListView>(locators[DropdownElement.List]);

        public IList<T> GetItems<T>() where T : IControl
        {
            return ((this.ItemsContainer == null ? this : this.ItemsContainer) as GuiControl)
                .FindList<T>(locators[DropdownElement.ListItem]);
        }

        public bool Expanded => GetPattern<ExpandCollapsePattern>().Current.ExpandCollapseState.Equals(ExpandCollapseState.Expanded);

        public virtual string SelectedValue => this.Input?.Value;

        public override ControlType UiaType => ControlType.ComboBox;

        public void Populate(Dictionary<int, ByLocator> elementsLocators)
        {
            foreach (var locator in elementsLocators)
            {
                locators[(DropdownElement)locator.Key] = locator.Value;
            }
        }

        public bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tCollapsing dropdown");
            if (!this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to collapse (collapsed by default)");
                return false;
            }

            this.ExpandCollapse.Click();
            Logger.Instance.Log(LogLevel.Trace, "\t\tCollapsed");
            return true;
        }

        public bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tExpanding dropdown");
            if (this.Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to expand (expanded by default)");
                return false;
            }

            this.ExpandCollapse.Click();
            Logger.Instance.Log(LogLevel.Trace, "\t\tExpanded");
            return true;
        }

        public virtual bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {this.ToString()}");

            if (itemName.Equals(this.SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (the item is selected by default)");
                return false;
            }

            Expand();
            var itemEl = this.GetItems<ListItem>().Where(i => i.Text.Equals(itemName));

            if (itemEl.Any())
            {
                Logger.Instance.Log(LogLevel.Trace, "\tItem was found. Selecting...");
                itemEl.First().Click();
            }
            else
            {
                throw new ControlNotFoundException($"Item '{itemName}' was not found");
            }

            Collapse();

            Logger.Instance.Log(LogLevel.Trace, "\tItem was selected");

            return true;
        }
    }
}
