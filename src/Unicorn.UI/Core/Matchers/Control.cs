using Unicorn.UI.Core.Controls.Interfaces;
using Unicorn.UI.Core.Matchers.IControlMatchers;
using Unicorn.UI.Core.Matchers.TypifiedMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for general control matchers.
    /// </summary>
    public static class Control
    {
        public static HasAttribute HasAttribute(string attribute)
            => new HasAttribute(attribute);

        public static ControlEnabledMatcher Enabled()
            => new ControlEnabledMatcher();

        public static ControlVisibleMatcher Visible()
            => new ControlVisibleMatcher();

        public static SelectedMatcher Selected()
            => new SelectedMatcher();

        public static SortedMatcher Sorted(SortDirection direction)
            => new SortedMatcher(direction);

        public static ControlHasTextMatcher HasText(string expectedText)
            => new ControlHasTextMatcher(expectedText);

        public static ControlHasTextMatchesMatcher HasTextMatching(string expectedTextRegex)
            => new ControlHasTextMatchesMatcher(expectedTextRegex);
    }
}
