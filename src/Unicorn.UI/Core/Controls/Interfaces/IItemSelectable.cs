namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for controls which have functionality of items selection.
    /// </summary>
    public interface IItemSelectable
    {
        /// <summary>
        /// Gets currently selected value.
        /// </summary>
        string SelectedValue
        {
            get;
        }

        /// <summary>
        /// Selects sub-item by name.
        /// </summary>
        /// <param name="itemName">sub-tem name</param>
        /// <returns>true - if selection was made, false - if the item is already selected</returns>
        bool Select(string itemName);
    }
}
