using Unicorn.Taf.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class CheckboxCheckedMatcher : TypeSafeMatcher<ICheckbox>
    {
        public CheckboxCheckedMatcher()
        {
        }

        public override string CheckDescription => "checked";

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
