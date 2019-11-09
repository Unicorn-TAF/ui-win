using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlHasTextMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _expectedText;

        public ControlHasTextMatcher(string expectedText)
        {
            _expectedText = expectedText;
        }

        public override string CheckDescription => $"has text = '{_expectedText}'";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualText = actual.Text;

            DescribeMismatch($"having text = '{actualText}'");

            return actualText.Equals(_expectedText);
        }
    }
}
