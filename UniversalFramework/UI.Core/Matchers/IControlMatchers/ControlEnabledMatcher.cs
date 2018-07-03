using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class ControlEnabledMatcher : TypeSafeMatcher<IControl>
    {
        public ControlEnabledMatcher()
        {
        }

        public override string CheckDescription => "enabled";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool enabled = actual.Enabled;

            if (enabled)
            {
                return true;
            }
            else
            {
                DescribeMismatch(enabled ? "enabled" : "disabled");
                return false;
            }
        }
    }
}
