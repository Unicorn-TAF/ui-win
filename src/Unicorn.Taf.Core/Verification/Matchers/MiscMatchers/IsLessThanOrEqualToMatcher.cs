using System;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IComparable"/> is less than or equal to other <see cref="IComparable"/>.
    /// </summary>
    public class IsLessThanOrEqualToMatcher : TypeSafeMatcher<IComparable>
    {
        private readonly IComparable _compareTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsLessThanOrEqualToMatcher"/> class with specified object to compare.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        public IsLessThanOrEqualToMatcher(IComparable compareTo)
        {
            _compareTo = compareTo;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Is less than or equal to {_compareTo}";

        /// <summary>
        /// Checks if <see cref="IComparable"/> is less than or equal to other.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if <see cref="IComparable"/> is less than or equal to other; otherwise - false</returns>
        public override bool Matches(IComparable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(actual.ToString());
            return actual.CompareTo(_compareTo) <= 0;
        }
    }
}
