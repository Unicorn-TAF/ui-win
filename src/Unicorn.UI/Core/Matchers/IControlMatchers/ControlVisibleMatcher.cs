using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control is visible. 
    /// </summary>
    public class ControlVisibleMatcher : TypeSafeMatcher<IControl>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControlVisibleMatcher"/> class.
        /// </summary>
        public ControlVisibleMatcher()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "visible";

        /// <summary>
        /// Checks if UI control is visible.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control is visible; otherwise - false</returns>
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
