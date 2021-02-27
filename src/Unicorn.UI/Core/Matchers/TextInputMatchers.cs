using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for Textinput matchers.
    /// </summary>
    public class TextInputMatchers
    {
        /// <summary>
        /// Gets matcher to check if Textinput has specified value.
        /// </summary>
        /// <param name="expectedValue">expected value</param>
        /// <returns>matcher instance</returns>
        public InputHasValueMatcher HasValue(string expectedValue) =>
            new InputHasValueMatcher(expectedValue);
    }
}
