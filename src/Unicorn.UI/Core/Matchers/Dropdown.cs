using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Dropdown
    {
        public static DropdownHasSelectedValueMatcher HasSelectedValue(string expectedValue)
            => new DropdownHasSelectedValueMatcher(expectedValue);

        public static DropdownExpandedMatcher Expanded()
            => new DropdownExpandedMatcher();
    }
}
