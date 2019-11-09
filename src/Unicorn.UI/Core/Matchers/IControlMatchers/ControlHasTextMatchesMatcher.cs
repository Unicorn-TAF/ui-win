using System.Text.RegularExpressions;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlHasTextMatchesMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _expectedTextRegex;

        public ControlHasTextMatchesMatcher(string expectedTextRegex)
        {
            _expectedTextRegex = expectedTextRegex;
        }

        public override string CheckDescription => $"has text matching expression '{_expectedTextRegex}'";

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
