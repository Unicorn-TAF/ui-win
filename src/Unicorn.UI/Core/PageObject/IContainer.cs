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
        /// <param name="name">button mane</param>
        void ClickButton(string name);

        /// <summary>
        /// Selects radio with specific name.
        /// </summary>
        /// <param name="name">radio button name</param>
        /// <returns>true - if selection was made; otherwise - false</returns>
        bool SelectRadio(string name);

        /// <summary>
        /// Sets specified state for checkbox with specified name.
        /// </summary>
        /// <param name="name">checkbox name</param>
        /// <param name="state">state to set</param>
        /// <returns>true - if selection was made; otherwise - false</returns>
        bool SetCheckbox(string name, bool state);

        /// <summary>
        /// Inputs text to text input with specified name.
        /// </summary>
        /// <param name="name">text input name</param>
        /// <param name="text">text to set</param>
        void InputText(string name, string text);
    }
}
