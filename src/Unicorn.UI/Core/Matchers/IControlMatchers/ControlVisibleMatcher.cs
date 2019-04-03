using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlVisibleMatcher : TypeSafeMatcher<IControl>
    {
        public ControlVisibleMatcher()
        {
        }

        public override string CheckDescription => "visible";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool visible = actual.Visible;
            DescribeMismatch(visible ? "visible" : "not visible");
            return visible;
        }
    }
}
