using System.Collections.Generic;
using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Controls.Dynamic
{
    /// <summary>
    /// UI dropdown sub-elements.
    /// </summary>
    public enum DropdownElement
    {
        /// <summary>
        /// Dropdown selection.
        /// </summary>
        ValueInput = 1,

        /// <summary>
        /// Dropdown expand/collapse
        /// </summary>
        ExpandCollapse = 2,

        /// <summary>
        /// Dropdown list.
        /// </summary>
        OptionsFrame = 3,

        /// <summary>
        /// Dropdown option.
        /// </summary>
        Option = 4,

        /// <summary>
        /// Dropdown content load indicator.
        /// </summary>
        Loader = 5
    }

    /// <summary>
    /// Interface for dynamically defined UI dropdown.
    /// </summary>
    public interface IDynamicDropdown : IDynamicControl, IDropdown, ILoadable
    {
        /// <summary>
        /// Gets dropdown selection control.
        /// </summary>
        ITextInput ValueInput { get; }

        /// <summary>
        /// Gets dropdown expand/collapse trigger control.
        /// </summary>
        IControl ExpandCollapse { get; }

        /// <summary>
        /// Gets dropdown list control.
        /// </summary>
        IControl OptionsFrame { get; }

        /// <summary>
        /// Gets list of controls for dropdown options.
        /// </summary>
        /// <returns></returns>
        IList<IControl> GetOptions();

        /// <summary>
        /// Gets specified dropdown option by name.
        /// </summary>
        /// <param name="optionName">option name</param>
        /// <returns>option control</returns>
        IControl GetOption(string optionName);

        /// <summary>
        /// Searches for specified option. If Loader sub-element is defined also waits for loader.
        /// </summary>
        /// <param name="optionName">option name</param>
        void SearchFor(string optionName);
    }
}
