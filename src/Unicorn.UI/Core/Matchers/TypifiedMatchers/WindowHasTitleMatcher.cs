using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IWindow"/> UI control has specified title. 
    /// </summary>
    public class WindowHasTitleMatcher : TypeSafeMatcher<IWindow>
    {
        private readonly string _expectedTitle;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowHasTitleMatcher"/> class.
        /// </summary>
        public WindowHasTitleMatcher(string expectedTitle)
        {
            _expectedTitle = expectedTitle;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has title '{_expectedTitle}'";

        /// <summary>
        /// Checks if window has specified title.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if window has specified title; otherwise - false</returns>
        public override bool Matches(IWindow actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualTitle = actual.Title;
            bool titleMatch = actualTitle.Equals(_expectedTitle);
            DescribeMismatch(actualTitle);
            return titleMatch;
        }
    }
}
