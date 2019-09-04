using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class SelectedMatcher : TypeSafeMatcher<ISelectable>
    {
        public SelectedMatcher()
        {
        }

        public override string CheckDescription => "is selected";

        public override bool Matches(ISelectable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool selected = actual.Selected;
            DescribeMismatch(selected ? "selected" : "not selected");
            return selected;
        }
    }
}
