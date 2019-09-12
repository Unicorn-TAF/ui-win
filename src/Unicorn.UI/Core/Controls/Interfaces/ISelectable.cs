namespace Unicorn.UI.Core.Controls.Interfaces
{
    /// <summary>
    /// Interface for controls which could be selected.
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Gets a value indicating whether control is selected.
        /// </summary>
        bool Selected
        {
            get;
        }

        /// <summary>
        /// Perform selection.
        /// </summary>
        /// <returns>true - if selection was made; false - if already selected</returns>
        bool Select();
    }
}
