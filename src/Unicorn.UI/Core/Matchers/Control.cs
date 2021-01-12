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
        /// <summary>
        /// Creates entry point for attribute matchers.
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <returns><see cref="HasAttribute"/> matchers entry point</returns>
        public static HasAttribute HasAttribute(string attribute)
            => new HasAttribute(attribute);

        /// <summary>
        /// Matcher to check if  UI control is enabled.
        /// </summary>
        /// <returns><see cref="ControlEnabledMatcher"/> instance</returns>
        public static ControlEnabledMatcher Enabled()
            => new ControlEnabledMatcher();

        /// <summary>
        /// Matcher to check if UI control is visible.
        /// </summary>
        /// <returns><see cref="ControlVisibleMatcher"/> instance</returns>
        public static ControlVisibleMatcher Visible()
            => new ControlVisibleMatcher();

        /// <summary>
        /// Matcher to check if <see cref="ISelectable"/> UI control is selected.
        /// </summary>
        /// <returns><see cref="SelectedMatcher"/> instance</returns>
        public static SelectedMatcher Selected()
            => new SelectedMatcher();

        /// <summary>
        /// Matcher to check if <see cref="ISortable"/> UI control is sorted by specified direction.
        /// </summary>
        /// <param name="direction">sorting direction</param>
        /// <returns><see cref="SortedMatcher"/> instance</returns>
        public static SortedMatcher Sorted(SortDirection direction)
            => new SortedMatcher(direction);

        /// <summary>
        /// Matcher to check if UI control has specific text.
        /// </summary>
        /// <param name="expectedTextRegex">expected control text</param>
        /// <returns><see cref="ControlHasTextMatcher"/> instance</returns>
        public static ControlHasTextMatcher HasText(string expectedText)
            => new ControlHasTextMatcher(expectedText);

        /// <summary>
        /// Matcher to check if UI control has text matching specific regular expression.
        /// </summary>
        /// <param name="expectedTextRegex">regular expression</param>
        /// <returns><see cref="ControlHasTextMatchesMatcher"/> instance</returns>
        public static ControlHasTextMatchesMatcher HasTextMatching(string expectedTextRegex)
            => new ControlHasTextMatchesMatcher(expectedTextRegex);
    }
}
