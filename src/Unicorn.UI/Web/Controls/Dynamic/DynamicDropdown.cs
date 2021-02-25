using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Controls.Interfaces.Typified;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Controls.Typified;

namespace Unicorn.UI.Web.Controls.Dynamic
{
    /// <summary>
    /// Describes dynamically defined dropdown (each sub-control could be defined using attribute).
    /// </summary>
    public class DynamicDropdown : WebControl, IDynamicDropdown
    {
        /// <summary>
        /// Gets a value indicating whether dropdown is expanded or not (options list is displayed).
        /// </summary>
        public virtual bool Expanded => 
            Locators.ContainsKey(DropdownElement.OptionsFrame) && 
            TryGetChild(Locators[DropdownElement.OptionsFrame], 0, out WebControl container) && 
            container.Visible;

        /// <summary>
        /// Gets dropdown selected value.
        /// </summary>
        public virtual string SelectedValue
        {
            get
            {
                if (Locators.ContainsKey(DropdownElement.ValueInput))
                {
                    var value = GetValueInput().Value;

                    if (value == null)
                    {
                        value = (GetValueInput() as TextInput).Text;
                    }

                    return value;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets dictionary of sub-elements locators.
        /// </summary>
        protected Dictionary<DropdownElement, ByLocator> Locators = new Dictionary<DropdownElement, ByLocator>();

        /// <summary>
        /// Populates sub-elements locators from input dictionary.
        /// </summary>
        /// <param name="elementsLocators">sub-elements locators dictionary</param>
        public void Populate(Dictionary<int, ByLocator> elementsLocators)
        {
            foreach (var locator in elementsLocators)
            {
                var key = (DropdownElement)locator.Key;

                if (Locators.ContainsKey(key))
                {
                    Locators[key] = locator.Value;
                }
                else
                {
                    Locators.Add((DropdownElement)locator.Key, locator.Value);
                }
            }
        }

        /// <summary>
        /// Gets control for expand/collapse trigger.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetExpandCollapse() =>
            Locators.ContainsKey(DropdownElement.ExpandCollapse) ?
            Find<WebControl>(Locators[DropdownElement.ExpandCollapse]) :
            throw new NotSpecifiedLocatorException("Expand/Collapse dropdown sub-control locator is not specified.");

        /// <summary>
        /// Gets dropdown input element with selected value.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="ITextInput"/> instance</returns>
        public virtual ITextInput GetValueInput() =>
            Locators.ContainsKey(DropdownElement.ValueInput) ?
            Find<TextInput>(Locators[DropdownElement.ValueInput]) :
            throw new NotSpecifiedLocatorException("Value input dropdown sub-control locator is not specified.");

        /// <summary>
        /// Gets control of dropdown options frame.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetOptionsFrame() =>
            Locators.ContainsKey(DropdownElement.OptionsFrame) ?
            Find<WebControl>(Locators[DropdownElement.OptionsFrame]) :
            throw new NotSpecifiedLocatorException("Options frame dropdown sub-control locator is not specified.");

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

            GetExpandCollapse().Click();

            if (Locators.ContainsKey(DropdownElement.OptionsFrame))
            {
                GetOptionsFrame().Wait(Until.Visible, TimeSpan.FromSeconds(10));
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

            GetExpandCollapse().Click();
            Logger.Instance.Log(LogLevel.Trace, "\t\tCollapsed");
            return true;
        }

        /// <summary>
        /// Gets list of controls for dropdown options.
        /// </summary>
        public virtual IList<IControl> GetOptions() =>
            FindList<WebControl>(Locators[DropdownElement.Option]).Cast<IControl>().ToList();

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
            var option = GetOption(itemName);

            try
            {
                option.Click();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                Logger.Instance.Log(LogLevel.Warning, "Got StaleElementReferenceException. Retrying...");
                option = GetOption(itemName);
                (option as WebControl).JsClick();
            }

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
            GetValueInput().SetValue(optionName);
            WaitForLoading(TimeSpan.FromSeconds(60));
        }

        /// <summary>
        /// Waits for dropdown loader appearance for 1.5 seconds and then for its disappearance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoaderTimeoutException">thrown if loader has not disappeared during timeout period</exception>
        public virtual bool WaitForLoading(TimeSpan timeout)
        {
            if (Locators.ContainsKey(DropdownElement.Loader))
            {
                new LoaderHandler(
                    () => TryGetChild<WebControl>(Locators[DropdownElement.Loader]),
                    () => !TryGetChild<WebControl>(Locators[DropdownElement.Loader]))
                .WaitFor(timeout);
            }

            return true;
        }
    }
}
