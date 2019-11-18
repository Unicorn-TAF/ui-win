using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces.Typified;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IDropdown"/> UI control is expanded. 
    /// </summary>
    public class DropdownExpandedMatcher : TypeSafeMatcher<IDropdown>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownExpandedMatcher"/> class.
        /// </summary>
        public DropdownExpandedMatcher()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "expanded";

        /// <summary>
        /// Checks if UI dropdown is expanded.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if dropdown is expanded; otherwise - false</returns>
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
