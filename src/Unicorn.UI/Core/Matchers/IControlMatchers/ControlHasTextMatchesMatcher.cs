using System.Text.RegularExpressions;
using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlHasTextMatchesMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string expectedTextRegex;

        public ControlHasTextMatchesMatcher(string expectedTextRegex)
        {
            this.expectedTextRegex = expectedTextRegex;
        }

        public override string CheckDescription => $"has text matching expression '{this.expectedTextRegex}'";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualText = actual.Text;

            DescribeMismatch($"having text = '{actualText}'");

            return new Regex(expectedTextRegex).IsMatch(actualText);
        }
    }
}
