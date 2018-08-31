using Unicorn.Core.Testing.Verification.Matchers;
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

            bool @checked = actual.Checked;
            DescribeMismatch(@checked ? "checked" : "not checked");
            return @checked;
        }
    }
}
