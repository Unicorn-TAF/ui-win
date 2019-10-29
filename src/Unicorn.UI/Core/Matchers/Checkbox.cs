using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for checkbox matchers.
    /// </summary>
    public static class Checkbox
    {
        /// <summary>
        /// Gets matcher to check if checkbox is checked.
        /// </summary>
        /// <returns>matcher instance</returns>
        public static CheckboxCheckedMatcher Checked()
            => new CheckboxCheckedMatcher();
    }
}
