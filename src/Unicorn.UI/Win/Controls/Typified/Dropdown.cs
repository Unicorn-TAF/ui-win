using System.Threading;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Win.Controls.Typified
{
    /// <summary>
    /// Describes base dropdown control.
    /// </summary>
    public class Dropdown : WinControl, IDropdown
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dropdown"/> class.
        /// </summary>
        public Dropdown()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dropdown"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        public Dropdown(IUIAutomationElement instance)
            : base(instance)
        {
        }

        /// <summary>
        /// Gets UIA combo box control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ComboBoxControlTypeId;

        /// <summary>
        /// Gets a value indicating whether dropdown is expanded.
        /// </summary>
        public virtual bool Expanded => 
            ExpandCollapsePattern
            .CurrentExpandCollapseState
            .Equals(ExpandCollapseState.ExpandCollapseState_Expanded);

        /// <summary>
        /// Gets dropdown current selected value.
        /// </summary>
        public virtual string SelectedValue
        {
            get
            {
                var selection = SelectionPattern;
                if (selection != null)
                {
                    var items = selection.GetCurrentSelection();

                    if (items.Length > 0)
                    {
                        return items.GetElement(0)
                            .GetCurrentPropertyValue(UIA_PropertyIds.UIA_NamePropertyId) as string;
                    }
                }

                var value = ValuePattern;

                if (value != null)
                {
                    return value.CurrentValue;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets expand/collapse pattern instance.
        /// </summary>
        protected IUIAutomationExpandCollapsePattern ExpandCollapsePattern => 
            Instance.GetPattern<IUIAutomationExpandCollapsePattern>();

        /// <summary>
        /// Gets selection pattern instance.
        /// </summary>
        protected IUIAutomationSelectionPattern SelectionPattern => 
            Instance.GetPattern<IUIAutomationSelectionPattern>();

        /// <summary>
        /// Gets value pattern instance.
        /// </summary>
        protected IUIAutomationValuePattern ValuePattern => 
            Instance.GetPattern<IUIAutomationValuePattern>();

        /// <summary>
        /// Selects specified item from dropdown.
        /// </summary>
        /// <param name="itemName">item to select</param>
        /// <returns>true - if item was selected; false - if specified item is already selected</returns>
        public virtual bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {ToString()}");

            if (itemName.Equals(SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            var value = ValuePattern;

            if (value != null)
            {
                value.SetValue(itemName);
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

            ExpandCollapsePattern.Expand();
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

            ExpandCollapsePattern.Collapse();
            Logger.Instance.Log(LogLevel.Trace, "Collapsed");
            return true;
        }
    }
}
