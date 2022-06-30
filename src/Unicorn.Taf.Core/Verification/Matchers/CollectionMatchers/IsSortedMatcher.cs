using System;
using System.Collections;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is sorted in desired direction. 
    /// </summary>
    public class IsSortedMatcher : TypeSafeMatcher<IEnumerable>
    {
        private readonly bool _ascending;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsSortedMatcher"/> class with specified direction.
        /// </summary>
        /// <param name="ascending">true for ascending order; false for descending</param>
        public IsSortedMatcher(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => "Is sorted " + (_ascending ? "ascending" : "descending");

        /// <summary>
        /// Checks if collection is sorted in desired direction.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection is sorted in desired direction; otherwise - false</returns>
        public override bool Matches(IEnumerable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            IEnumerator enumerator = actual.GetEnumerator();

            IComparable previous = null;

            int index = 0;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current is IComparable c)
                {
                    if (previous != null && !IsRightOrder(previous, c))
                    {
                        DescribeMismatch($"Sorting is not valid at index {index} ({previous} >> {c})");
                        return false;
                    }

                    previous = c;
                    index++;
                }
                else
                {
                    DescribeMismatch("having non-comparable items");
                    return Reverse;
                }
            }

            return true;
        }

        private bool IsRightOrder(IComparable item1, IComparable item2) =>
            _ascending ? item1.CompareTo(item2) <= 0 :
                item1.CompareTo(item2) >= 0;
    }
}
