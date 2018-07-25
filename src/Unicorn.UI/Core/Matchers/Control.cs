using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Control
    {
        public static HasAttribute HasAttribute(string attribute)
        {
            return new HasAttribute(attribute);
        }

        public static ControlEnabledMatcher Enabled()
        {
            return new ControlEnabledMatcher();
        }

        public static ControlVisibleMatcher Visible()
        {
            return new ControlVisibleMatcher();
        }

        public static ControlHasTextMatcher HasText(string expectedText)
        {
            return new ControlHasTextMatcher(expectedText);
        }
    }
}
