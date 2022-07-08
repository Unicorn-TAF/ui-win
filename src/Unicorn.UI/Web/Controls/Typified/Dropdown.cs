using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Web.Controls.Typified
{
    /// <summary>
    /// Default implementation for Web dropdown described by &lt;select&gt; tag. 
    /// Has definitions of of basic methods and properties.
    /// </summary>
    public class Dropdown : WebControl, IDropdown
    {
        private SelectElement selectInstance = null;

        /// <summary>
        /// Gets a value indicating whether dropdown is expanded (always false for base implementation).
        /// </summary>
        public bool Expanded => false;

        /// <summary>
        /// Gets currently selected value.
        /// </summary>
        public string SelectedValue => SelectControl.SelectedOption.Text;

        private SelectElement SelectControl
        {
            get
            {
                if (selectInstance == null)
                {
                    selectInstance = new SelectElement(Instance);
                }

                return selectInstance;
            }
        }

        /// <summary>
        /// Perform collapsing.
        /// </summary>
        /// <returns>true - if collapse was performed; false - if already collapsed</returns>
        public bool Collapse() =>
            throw new NotImplementedException();

        /// <summary>
        /// Perform expanding.
        /// </summary>
        /// <returns>true - if expand was performed; false - if already expanded</returns>
        public bool Expand() =>
            throw new NotImplementedException();

        /// <summary>
        /// Selects dropdown option by name.
        /// </summary>
        /// <param name="itemName">item name</param>
        /// <returns>true - if selection was made, false - if the item is already selected</returns>
        public bool Select(string itemName)
        {
            if (itemName == null)
            {
                throw new ArgumentNullException(nameof(itemName));
            }

            Logger.Instance.Log(LogLevel.Debug, $"Select '{itemName}' item from {this}");

            if (SelectedValue.Equals(itemName))
            {
                Logger.Instance.Log(LogLevel.Trace, "No need to select (the item is selected by default)");
                return false;
            }

            SelectControl.SelectByText(itemName);
            Logger.Instance.Log(LogLevel.Trace, "Item was selected");

            return true;
        }

        /// <summary>
        /// Gets all dropdown options.
        /// </summary>
        /// <returns>string array with options</returns>
        public string[] GetOptions() => 
            SelectControl.Options.Select(o => o.Text).ToArray();
    }
}
