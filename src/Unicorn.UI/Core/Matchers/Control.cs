using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class Control
    {
        public static HasAttribute HasAttribute(string attribute)
            => new HasAttribute(attribute);

        public static ControlEnabledMatcher Enabled()
            => new ControlEnabledMatcher();

        public static ControlVisibleMatcher Visible()
            => new ControlVisibleMatcher();

        public static ControlHasTextMatcher HasText(string expectedText)
            => new ControlHasTextMatcher(expectedText);

        public static ControlHasTextMatchesMatcher HasTextMatching(string expectedTextRegex)
            => new ControlHasTextMatchesMatcher(expectedTextRegex);
    }
}
