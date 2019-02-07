using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    public enum DropdownElement
    {
        TextInput = 1,
        ExpandCollapse = 2,
        List = 3,
        ListItem = 4
    }

    public abstract class DynamicDropdown : IDynamicControl<DropdownElement>, IDropdown
    {
        public abstract ITextInput Input { get; }

        public abstract IControl ExpandCollapse { get; }

        public abstract IList<IControl> Items { get; }

        public abstract IControl ItemsContainer { get; }

        public void Populate(Dictionary<DropdownElement, ByLocator> elementsLocators)
        {
            throw new NotImplementedException();
        }

        public abstract bool Expanded { get; }

        public virtual string SelectedValue =>
            this.Input.Value;

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
            var itemEl = this.Items.Where(i => i.Text.Equals(itemName));

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
