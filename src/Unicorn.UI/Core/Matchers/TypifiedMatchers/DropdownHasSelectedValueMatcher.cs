using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class DropdownHasSelectedValueMatcher : TypeSafeMatcher<IDropdown>
    {
        private readonly string expectedValue;

        public DropdownHasSelectedValueMatcher(string expectedValue)
        {
            this.expectedValue = expectedValue;
        }

        public override string CheckDescription => $"has selected value '{this.expectedValue}'";

        public override bool Matches(IDropdown actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string selectedValue = actual.SelectedValue;
            bool hasValue = selectedValue.Equals(this.expectedValue);
            DescribeMismatch($"having value '{selectedValue}'");
            return hasValue;
        }
    }
}
