using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IModalWindow"/> UI control has specified text content. 
    /// </summary>
    public class ModalWindowHasTextMatcher : TypeSafeMatcher<IModalWindow>
    {
        private readonly string _expectedText;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModalWindowHasTextMatcher"/> class.
        /// </summary>
        public ModalWindowHasTextMatcher(string expectedText)
        {
            _expectedText = expectedText;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has text '{_expectedText}'";

        /// <summary>
        /// Checks if modal window has specified text content.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if window has specified text content; otherwise - false</returns>
        public override bool Matches(IModalWindow actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualText = actual.TextContent;
            bool hasText = actualText.Equals(_expectedText);
            DescribeMismatch(actualText);
            return hasText;
        }
    }
}
