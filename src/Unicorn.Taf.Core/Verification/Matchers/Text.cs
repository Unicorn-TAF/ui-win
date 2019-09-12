using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification
{
    public static class Text
    {
        public static StringMatchesRegexMatcher MatchesRegex(string regex) =>
            new StringMatchesRegexMatcher(regex);

        public static StringContainsMatcher Contains(string substring) =>
            new StringContainsMatcher(substring);
    }
}
