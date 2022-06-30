using System;

namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    /// <summary>
    /// Matcher to check if <see cref="IComparable"/> is greater than other <see cref="IComparable"/>.
    /// </summary>
    public class IsGreaterThanMatcher : TypeSafeMatcher<IComparable>
    {
        private readonly IComparable _compareTo;

        /// <summary>
        /// Initializes a new instance of the <see cref="IsGreaterThanMatcher"/> class with specified object to compare.
        /// </summary>
        /// <param name="compareTo">object to compare</param>
        public IsGreaterThanMatcher(IComparable compareTo)
        {
            _compareTo = compareTo;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Is greater than {_compareTo}";

        /// <summary>
        /// Checks if <see cref="IComparable"/> is greater than other.
        /// </summary>
        /// <param name="actual">object under assertion</param>
        /// <returns>true - if <see cref="IComparable"/> is greater than other; otherwise - false</returns>
        public override bool Matches(IComparable actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(actual.ToString());
            return actual.CompareTo(_compareTo) > 0;
        }
    }
}
