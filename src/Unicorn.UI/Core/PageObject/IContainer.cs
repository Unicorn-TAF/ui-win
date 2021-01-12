namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Interface for controls container.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Clicks button with specified name.
        /// </summary>
        /// <param name="locator">button mane</param>
        void ClickButton(string locator);

        /// <summary>
        /// Selects radio with specific name.
        /// </summary>
        /// <param name="locator">radio button name</param>
        /// <returns>true - if selection was made; otherwise - false</returns>
        bool SelectRadio(string locator);

        /// <summary>
        /// Sets specified state for checkbox with specified name.
        /// </summary>
        /// <param name="locator">checkbox name</param>
        /// <param name="state">state to set</param>
        /// <returns>true - if selection was made; otherwise - false</returns>
        bool SetCheckbox(string locator, bool state);

        /// <summary>
        /// Inputs text to text input with specified name.
        /// </summary>
        /// <param name="locator">text input name</param>
        /// <param name="text">text to set</param>
        void InputText(string locator, string text);
    }
}
