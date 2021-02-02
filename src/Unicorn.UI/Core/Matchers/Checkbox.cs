using System;
using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for checkbox matchers.
    /// </summary>
    [Obsolete("Please use Unicorn.UI.Core.Matchers.Ui entry point")]
    public static class Checkbox
    {
        /// <summary>
        /// Gets matcher to check if checkbox is checked.
        /// </summary>
        /// <returns>matcher instance</returns>
        public static CheckboxCheckedMatcher Checked()
            => new CheckboxCheckedMatcher();

        /// <summary>
        /// Gets matcher to check if checkbox has desired check state.
        /// </summary>
        /// <returns>matcher instance</returns>
        public static CheckboxHasCheckStateMatcher HasCheckState(bool isChecked)
            => new CheckboxHasCheckStateMatcher(isChecked);
    }
}
