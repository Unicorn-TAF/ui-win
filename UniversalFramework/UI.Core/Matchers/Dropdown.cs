using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Dropdown
    {
        public static DropdownHasSelectedValue HasSelectedValue(string expectedValue)
        {
            return new DropdownHasSelectedValue(expectedValue);
        }
    }
}
