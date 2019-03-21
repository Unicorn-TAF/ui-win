using Unicorn.Taf.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlHasTextMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string expectedText;

        public ControlHasTextMatcher(string expectedText)
        {
            this.expectedText = expectedText;
        }

        public override string CheckDescription => $"has text = '{this.expectedText}'";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualText = actual.Text;

            DescribeMismatch($"having text = '{actualText}'");

            return actualText.Equals(this.expectedText);
        }
    }
}
