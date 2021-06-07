using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for checkbox matchers.
    /// </summary>
    public class CheckboxMatchers
    {
        /// <summary>
        /// Gets matcher to check if checkbox is checked.
        /// </summary>
        /// <returns>matcher instance</returns>
        public CheckboxCheckedMatcher Checked() => 
            new CheckboxCheckedMatcher();

        /// <summary>
        /// Gets matcher to check if checkbox has desired check state.
        /// </summary>
        /// <returns>matcher instance</returns>
        public CheckboxHasCheckStateMatcher HasCheckState(bool isChecked) => 
            new CheckboxHasCheckStateMatcher(isChecked);
    }
}
