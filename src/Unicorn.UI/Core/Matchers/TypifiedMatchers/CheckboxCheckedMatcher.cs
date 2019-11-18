using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="ICheckbox"/> UI control is checked. 
    /// </summary>
    public class CheckboxCheckedMatcher : TypeSafeMatcher<ICheckbox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckboxCheckedMatcher"/> class.
        /// </summary>
        public CheckboxCheckedMatcher()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "is checked";

        /// <summary>
        /// Checks if checkbox is checked.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if checkbox is checked; otherwise - false</returns>
        public override bool Matches(ICheckbox actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool isChecked = actual.Checked;
            DescribeMismatch(isChecked ? "checked" : "not checked");
            return isChecked;
        }
    }
}
