namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    /// <summary>
    /// Interface for checkboxes implementation. 
    /// Has definitions of of basic methods and properties.
    /// </summary>
    public interface ICheckbox
    {
        /// <summary>
        /// Gets a value indicating whether checkbox is checked.
        /// </summary>
        bool Checked
        {
            get;
        }

        /// <summary>
        /// Set specified state of checkbox.
        /// </summary>
        /// <param name="isChecked">true - to check the checkbox; false - to uncheck</param>
        /// <returns>true - if state was changed; false - if already in specified state</returns>
        bool SetCheckedState(bool isChecked);
    }
}
