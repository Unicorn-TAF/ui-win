using System.Collections.Generic;

namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for controls which have functionality of multi items selection.
    /// </summary>
    public interface IMultiItemSelectable
    {
        /// <summary>
        /// Gets currently selected values.
        /// </summary>
        List<string> SelectedValues
        {
            get;
        }

        /// <summary>
        /// Selects sub-items by their names.
        /// </summary>
        /// <param name="itemName">sub-items names</param>
        /// <returns>true - if selection was made, false - if all of the items are already selected</returns>
        bool SelectMultiple(params string[] itemName);
    }
}
