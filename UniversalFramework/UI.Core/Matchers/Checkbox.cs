using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Checkbox
    {
        public static CheckboxCheckedMatcher Checked()
        {
            return new CheckboxCheckedMatcher();
        }
    }
}
