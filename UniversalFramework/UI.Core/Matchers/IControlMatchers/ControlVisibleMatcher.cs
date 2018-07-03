using Unicorn.Core.Testing.Verification.Matchers;
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

            if (visible)
            {
                return true;
            }
            else
            {
                DescribeMismatch(visible ? "visible" : "not visible");
                return false;
            }
        }
    }
}
