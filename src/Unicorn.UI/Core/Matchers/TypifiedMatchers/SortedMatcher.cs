using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls.Interfaces;

namespace Unicorn.UI.Core.Matchers.TypifiedMatchers
{
    public class SortedMatcher : TypeSafeMatcher<ISortable>
    {
        private readonly SortDirection _sortDirection;

        public SortedMatcher(SortDirection sortDirection)
        {
            _sortDirection = sortDirection;
        }

        public override string CheckDescription => $"is sorted {_sortDirection}";

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
