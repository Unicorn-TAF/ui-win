using System.Text.RegularExpressions;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    public class StringMatchesRegexMatcher : TypeSafeMatcher<string>
    {
        private readonly string regex;

        public StringMatchesRegexMatcher(string regex)
        {
            this.regex = regex;
        }

        public override string CheckDescription => $"Matches regex '{this.regex}'";

        public override bool Matches(string actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            this.DescribeMismatch(actual);
            return Regex.IsMatch(actual, this.regex);
        }
    }
}
