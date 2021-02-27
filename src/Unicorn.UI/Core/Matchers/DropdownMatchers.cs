using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for dropdown matchers.
    /// </summary>
    public class DropdownMatchers
    {
        /// <summary>
        /// Gets matcher to check if dropdown contains specified item.
        /// </summary>
        /// <param name="expectedValue">expected dropdown item</param>
        /// <returns>matcher instance</returns>
        public DropdownHasSelectedValueMatcher HasSelectedValue(string expectedValue) =>
            new DropdownHasSelectedValueMatcher(expectedValue);

        /// <summary>
        /// Gets matcher to check if dropdown is expanded.
        /// </summary>
        /// <returns>matcher instance</returns>
        public DropdownExpandedMatcher Expanded() =>
            new DropdownExpandedMatcher();
    }
}
