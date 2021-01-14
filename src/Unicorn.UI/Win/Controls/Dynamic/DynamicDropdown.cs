using System;
using System.Collections.Generic;
using System.Linq;
using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Win.Controls.Typified;

namespace Unicorn.UI.Win.Controls.Dynamic
{
    /// <summary>
    /// Describes dynamically defined dropdown (each sub-control could be defined using attribute).
    /// </summary>
    public class DynamicDropdown : WinControl, IDynamicDropdown
    {
        /// <summary>
        /// Gets dictionary of dd elements locators.
        /// </summary>
        protected Dictionary<DropdownElement, ByLocator> locators = new Dictionary<DropdownElement, ByLocator>();

        /// <summary>
        /// Gets or sets control for expand/collapse trigger.
        /// </summary>
        public virtual IControl ExpandCollapse => locators.ContainsKey(DropdownElement.ExpandCollapse) ?
            Find<Button>(locators[DropdownElement.ExpandCollapse]) :
            throw new ControlNotFoundException($"{nameof(ExpandCollapse)} dropdown sub-control locator is not specified.");

        /// <summary>
        /// Gets or sets dropdown input element with selected value.
        /// </summary>
        public virtual ITextInput ValueInput => locators.ContainsKey(DropdownElement.ValueInput) ?
            Find<TextInput>(locators[DropdownElement.ValueInput]) :
            throw new ControlNotFoundException($"{nameof(ValueInput)} dropdown sub-control locator is not specified.");

        /// <summary>
        /// Gets or sets control of dropdown options frame.
        /// </summary>
        public virtual IControl OptionsFrame => locators.ContainsKey(DropdownElement.OptionsFrame) ?
            Find<ListView>(locators[DropdownElement.OptionsFrame]) :
            throw new ControlNotFoundException($"{nameof(OptionsFrame)} dropdown sub-control locator is not specified.");

        /// <summary>
        /// Gets a value indicating whether dropdown is expanded or not (options list is displayed).
        /// </summary>
        public virtual bool Expanded =>
            Instance
            .GetPattern<IUIAutomationExpandCollapsePattern>()
            .CurrentExpandCollapseState
            .Equals(ExpandCollapseState.ExpandCollapseState_Expanded);

        /// <summary>
        /// Gets dropdown selected value.
        /// </summary>
        public virtual string SelectedValue => ValueInput?.Value;

        /// <summary>
        /// Gets UIA control type.
        /// </summary>
        public override int UiaType => UIA_ControlTypeIds.UIA_ComboBoxControlTypeId;

        /// <summary>
        /// Populates sub-elements locators from input dictionary.
        /// </summary>
        /// <param name="elementsLocators">sub-elements locators dictionary</param>
        public void Populate(Dictionary<int, ByLocator> elementsLocators)
        {
            foreach (var locator in elementsLocators)
            {
                var key = (DropdownElement)locator.Key;

                if (locators.ContainsKey(key))
                {
                    locators[key] = locator.Value;
                }
                else
                {
                    locators.Add((DropdownElement)locator.Key, locator.Value);
                }
            }
        }

        /// <summary>
        /// Expands the dropdown (if it was not expanded) and waits for content loading.<para/>
        /// If items list is defined waits for its visibility.
        /// </summary>
        /// <returns>true - if dropdown has expanded; false - if dropdown was already expanded</returns>
        public virtual bool Expand()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tExpanding dropdown");

            if (Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to expand (expanded by default)");
                return false;
            }

            ExpandCollapse.Click();

            if (locators.ContainsKey(DropdownElement.OptionsFrame))
            {
                OptionsFrame.Wait(Until.Visible, TimeSpan.FromSeconds(10));
            }

            WaitForLoading(TimeSpan.FromSeconds(60));

            Logger.Instance.Log(LogLevel.Trace, "\t\tExpanded");
            return true;
        }

        /// <summary>
        /// Collapses the dropdown (if it was not collapsed).
        /// </summary>
        /// <returns>true - if dropdown has collapsed; false - if dropdown was already collapsed</returns>
        public virtual bool Collapse()
        {
            Logger.Instance.Log(LogLevel.Trace, "\tCollapsing dropdown");

            if (!Expanded)
            {
                Logger.Instance.Log(LogLevel.Trace, "\t\tNo need to collapse (collapsed by default)");
                return false;
            }

            ExpandCollapse.Click();
            Logger.Instance.Log(LogLevel.Trace, "\t\tCollapsed");
            return true;
        }

        /// <summary>
        /// Gets list of controls for dropdown options.
        /// </summary>
        public virtual IList<IControl> GetOptions() =>
            FindList<ListItem>(locators[DropdownElement.Option]).Cast<IControl>().ToList();

        /// <summary>
        /// Get dropdown option its name.
        /// </summary>
        /// <param name="optionName">option name</param>
        /// <returns>option control</returns>
        /// <exception cref="ControlNotFoundException">thrown if specified option was not found</exception>
        public virtual IControl GetOption(string optionName)
        {
            var options = GetOptions().Where(o => o.Text == optionName);
            return options.Any() ? options.First() : throw new ControlNotFoundException($"Unable to find '{optionName}' dropdown option");
        }

        /// <summary>
        /// If specified value is not already selected expands the dropdown,
        /// waits for content loading, selects specified value and collapses the dropdown.
        /// </summary>
        /// <param name="itemName">option to select</param>
        /// <returns>true - if option has selected; false - if option was already selected</returns>
        /// <exception cref="ControlNotFoundException">thrown if specified option was not found</exception>
        public virtual bool Select(string itemName)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {this}");

            if (itemName.Equals(SelectedValue))
            {
                Logger.Instance.Log(LogLevel.Trace, "\tNo need to select (the item is selected by default)");
                return false;
            }

            Expand();

            GetOption(itemName).Click();

            Collapse();

            Logger.Instance.Log(LogLevel.Trace, "\tItem has selected");

            return true;
        }

        /// <summary>
        /// Searches for specific option using search input.
        /// </summary>
        /// <param name="optionName">option name</param>
        /// <exception cref="ControlNotFoundException">thrown if no value input found</exception>
        public virtual void SearchFor(string optionName)
        {
            ValueInput.SetValue(optionName);
            WaitForLoading(TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// Waits for dropdown loader appearance for 1.5 seconds and then for its disappearance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoaderTimeoutException">thrown if loader has not disappeared during timeout period</exception>
        public virtual bool WaitForLoading(TimeSpan timeout)
        {
            if (locators.ContainsKey(DropdownElement.Loader))
            {
                new LoaderHandler(
                    () => TryGetChild<WinControl>(locators[DropdownElement.Loader]),
                    () => !TryGetChild<WinControl>(locators[DropdownElement.Loader]))
                .WaitFor(TimeSpan.FromSeconds(60));
            }

            return true;
        }
    }
}
