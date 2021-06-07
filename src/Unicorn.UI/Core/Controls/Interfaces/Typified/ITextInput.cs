namespace Unicorn.UI.Core.Controls.Interfaces.Typified
{
    /// <summary>
    /// Interface for text inputs (text edits) implementation. 
    /// Has definitions of of basic methods and properties.
    /// </summary>
    public interface ITextInput
    {
        /// <summary>
        /// Gets text input value.
        /// </summary>
        string Value
        {
            get;
        }

        /// <summary>
        /// Send keys (add text) to already existing input value.
        /// </summary>
        /// <param name="text">text to add</param>
        void SendKeys(string text);

        /// <summary>
        /// Set text input value.
        /// </summary>
        /// <param name="text">value to set</param>
        /// <returns>true - if value was set, false - if text input already has specified value</returns>
        bool SetValue(string text);
    }
}
