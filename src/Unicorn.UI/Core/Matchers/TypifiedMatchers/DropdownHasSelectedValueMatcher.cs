using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDropdown"/> UI control has specified value selected. 
    /// </summary>
    public class DropdownHasSelectedValueMatcher : TypeSafeMatcher<IDropdown>
    {
        private readonly string _expectedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownHasSelectedValueMatcher"/> class for specified selected value.
        /// </summary>
        public DropdownHasSelectedValueMatcher(string expectedValue)
        {
            _expectedValue = expectedValue;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has selected value '{_expectedValue}'";

        /// <summary>
        /// Checks if UI dropdown has specified value selected.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if dropdown has specified value selected; otherwise - false</returns>
        public override bool Matches(IDropdown actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var selectedValue = actual.SelectedValue;
            DescribeMismatch($"having value '{selectedValue}'");
            return selectedValue.Equals(_expectedValue);
        }
    }
}
