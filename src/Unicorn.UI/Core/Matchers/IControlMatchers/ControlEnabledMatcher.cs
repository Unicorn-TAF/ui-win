using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control is enabled. 
    /// </summary>
    public class ControlEnabledMatcher : TypeSafeMatcher<IControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlEnabledMatcher"/> class.
        /// </summary>
        public ControlEnabledMatcher()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "enabled";

        /// <summary>
        /// Checks if UI control is enabled.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control is enabled; otherwise - false</returns>
        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            bool enabled = actual.Enabled;

            DescribeMismatch(enabled ? "enabled" : "disabled");

            return enabled;
        }
    }
}
