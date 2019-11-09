using System.Text.RegularExpressions;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if string matches specified regular expression. 
    /// </summary>
    public class StringMatchesRegexMatcher : TypeSafeMatcher<string>
    {
        private readonly string _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringMatchesRegexMatcher"/> class for specified regex.
        /// </summary>
        /// <param name="regex">regex to match against</param>
        public StringMatchesRegexMatcher(string regex)
        {
            _regex = regex;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Matches regex '{_regex}'";

        /// <summary>
        /// Checks if target string matches specified regex.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if string matches specified regex; otherwise - false</returns>
        public override bool Matches(string actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(actual);
            return Regex.IsMatch(actual, _regex);
        }
    }
}
