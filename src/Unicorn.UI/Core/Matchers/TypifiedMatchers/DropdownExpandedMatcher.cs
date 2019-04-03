using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class DropdownExpandedMatcher : TypeSafeMatcher<IDropdown>
    {
        public DropdownExpandedMatcher()
        {
        }

        public override string CheckDescription => "expanded";

        public override bool Matches(IDropdown actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool expanded = actual.Expanded;
            DescribeMismatch(expanded ? "expanded" : "collapsed");
            return expanded;
        }
    }
}
