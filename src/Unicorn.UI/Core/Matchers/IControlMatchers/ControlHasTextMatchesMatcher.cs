using System.Text.RegularExpressions;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control has text matching specified regular expression. 
    /// </summary>
    public class ControlHasTextMatchesMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _expectedTextRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlHasTextMatchesMatcher"/> class for specified expected regular expression.
        /// </summary>
        public ControlHasTextMatchesMatcher(string expectedTextRegex)
        {
            _expectedTextRegex = expectedTextRegex;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has text matching expression '{_expectedTextRegex}'";

        /// <summary>
        /// Checks if UI control has text matching specified regular expression.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control has text matching specified regular expression; otherwise - false</returns>
        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualText = actual.Text;

            DescribeMismatch($"having text = '{actualText}'");

            return new Regex(_expectedTextRegex).IsMatch(actualText);
        }
    }
}
