using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="ISortable"/> UI control is sorted in specified direction. 
    /// </summary>
    public class SortedMatcher : TypeSafeMatcher<ISortable>
    {
        private readonly SortDirection _sortDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedMatcher"/> class for specified sort direction.
        /// </summary>
        public SortedMatcher(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"is sorted {_sortDirection}";

        /// <summary>
        /// Checks if UI control is sorted in specified direction.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control is sorted in specified direction; otherwise - false</returns>
        public override bool Matches(ISortable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch("not sorted correctly");
            return actual.IsSorted(_sortDirection);
        }
    }
}
