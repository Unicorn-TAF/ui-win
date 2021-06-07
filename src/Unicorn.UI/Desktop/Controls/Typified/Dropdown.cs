using System;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Desktop.Controls.Typified
{
    /// <summary>
    /// Describes base dropdown control.
    /// </summary>
    public class Dropdown : GuiControl, IDropdown
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dropdown"/> class.
        /// </summary>
        public Dropdown()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dropdown"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        public Dropdown(AutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA combo box control type.
        /// </summary>
        public override ControlType UiaType => ControlType.ComboBox;

        /// <summary>
        /// Gets a value indicating whether dropdown is expanded.
        /// </summary>
        public virtual bool Expanded =>
            Instance
            .GetPattern<ExpandCollapsePattern>()
            .Current
            .ExpandCollapseState
            .Equals(ExpandCollapseState.Expanded);

        /// <summary>
        /// Gets dropdown current selected value.
        /// </summary>
        public virtual string SelectedValue
        {
            get
            {
                var selection = Instance.GetPattern<SelectionPattern>();
                if (selection != null)
                {
                    var item = selection.Current.GetSelection().FirstOrDefault();

                    if (item != null)
                    {
                        return item.GetCurrentPropertyValue(AutomationElement.NameProperty) as string;
                    }
                }

                var value = Instance.GetPattern<ValuePattern>();

                if (value != null)
                {
                    return value.Current.Value;
                }
                    
                return string.Empty;
            }
        }

        /// <summary>
        /// Selects specified item from dropdown.
        /// </summary>
        /// <param name="itemName">item to select</param>
        /// <returns>true - if item was selected; false - if specified item is already selected</returns>
        public virtual bool Select(string itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }

            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {ToString()}");

            if (itemName.Equals(SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            var valuePattern = Instance.GetPattern<ValuePattern>();

            if (valuePattern != null)
            {
                valuePattern.SetValue(itemName);
            }
            else
            {
                Expand();
                Thread.Sleep(500);
                var itemEl = Find<ListItem>(ByLocator.Name(itemName));

                Logger.Instance.Log(LogLevel.Trace, "Item was found. Selecting...");
                itemEl.Select();
                    
                Collapse();
                Thread.Sleep(500);
            }

            Logger.Instance.Log(LogLevel.Trace, "Item was selected");

            return true;
        }

        /// <summary>
        /// Expands the dropdown.
        /// </summary>
        /// <returns>true - if expanding was performed; false - if already expanded</returns>
        public virtual bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "Expanding dropdown");
            if (Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to expand (expanded by default)");
                return false;
            }

            Instance.GetPattern<ExpandCollapsePattern>().Expand();
            Logger.Instance.Log(LogLevel.Trace, "Expanded");
            return true;
        }

        /// <summary>
        /// Collapses the dropdown.
        /// </summary>
        /// <returns>true - if collapsing was performed; false - if already collapsed</returns>
        public virtual bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "Collapsing dropdown");
            if (!Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to collapse (collapsed by default)");
                return false;
            }

            Instance.GetPattern<ExpandCollapsePattern>().Collapse();
            Logger.Instance.Log(LogLevel.Trace, "Collapsed");
            return true;
        }
    }
}
