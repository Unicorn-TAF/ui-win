using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Entry point for strings matchers.
    /// </summary>
    public static class Text
    {
        /// <summary>
        /// Matcher to check if string matches specified regex.
        /// </summary>
        /// <param name="regex">regex expression to check match</param>
        /// <returns><see cref="StringMatchesRegexMatcher"/> instance</returns>
        public static StringMatchesRegexMatcher MatchesRegex(string regex) =>
            new StringMatchesRegexMatcher(regex);

        /// <summary>
        /// Matcher to check if string contains specified sub-string.
        /// </summary>
        /// <param name="substring">expected sub-string</param>
        /// <returns><see cref="StringContainsMatcher"/> instance</returns>
        public static StringContainsMatcher Contains(string substring) =>
            new StringContainsMatcher(substring);
    }
}
