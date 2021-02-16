using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="ICheckbox"/> UI control has desired check state. 
    /// </summary>
    public class CheckboxHasCheckStateMatcher : TypeSafeMatcher<ICheckbox>
    {
        private readonly bool _isChecked;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckboxHasCheckStateMatcher"/> class with expected state.
        /// </summary>
        public CheckboxHasCheckStateMatcher(bool isChecked)
        {
            _isChecked = isChecked;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => _isChecked ? "is checked" : "is unchecked";

        /// <summary>
        /// Checks if checkbox is checked.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if checkbox checked state is equal to expected; otherwise - false</returns>
        public override bool Matches(ICheckbox actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool isChecked = actual.Checked;
            DescribeMismatch(isChecked ? "checked" : "unchecked");
            return isChecked.Equals(_isChecked);
        }
    }
}
