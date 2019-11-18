using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control has specified text. 
    /// </summary>
    public class ControlHasTextMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _expectedText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlHasTextMatcher"/> class for specified expected text.
        /// </summary>
        public ControlHasTextMatcher(string expectedText)
        {
            _expectedText = expectedText;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has text = '{_expectedText}'";

        /// <summary>
        /// Checks if UI control has specified text.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control has specified text; otherwise - false</returns>
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
