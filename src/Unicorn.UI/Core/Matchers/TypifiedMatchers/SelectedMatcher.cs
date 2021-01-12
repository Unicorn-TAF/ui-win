using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="ISelectable"/> UI control is selected. 
    /// </summary>
    public class SelectedMatcher : TypeSafeMatcher<ISelectable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedMatcher"/> class.
        /// </summary>
        public SelectedMatcher()
        {
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "is selected";

        /// <summary>
        /// Checks if UI control is selected.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control is selected; otherwise - false</returns>
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
