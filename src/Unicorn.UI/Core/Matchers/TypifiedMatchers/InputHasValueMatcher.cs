using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="ITextInput"/> UI control has specified value. 
    /// </summary>
    public class InputHasValueMatcher : TypeSafeMatcher<ITextInput>
    {
        private readonly string _expectedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckboxHasCheckStateMatcher"/> class with expected value.
        /// </summary>
        public InputHasValueMatcher(string isChecked)
        {
            _expectedValue = isChecked;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has value '{_expectedValue}'";

        /// <summary>
        /// Checks if text input has specified value.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if text input has expected value; otherwise - false</returns>
        public override bool Matches(ITextInput actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var actualValue = actual.Value;
            DescribeMismatch(actualValue);
            return actualValue.Equals(_expectedValue);
        }
    }
}
