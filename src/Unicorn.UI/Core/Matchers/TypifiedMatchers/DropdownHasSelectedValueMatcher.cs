using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class DropdownHasSelectedValueMatcher : TypeSafeMatcher<IDropdown>
    {
        private readonly string _expectedValue;

        public DropdownHasSelectedValueMatcher(string expectedValue)
        {
            _expectedValue = expectedValue;
        }

        public override string CheckDescription => $"has selected value '{_expectedValue}'";

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
