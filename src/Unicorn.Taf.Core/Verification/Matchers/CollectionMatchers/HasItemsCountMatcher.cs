using System.Collections;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection has specified items count.
    /// </summary>
    public class HasItemsCountMatcher : TypeSafeMatcher<ICollection>
    {
        private readonly int _expectedCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasItemsCountMatcher"/> class 
        /// with specified expected objects count.
        /// </summary>
        /// <param name="expectedCount">expected items count</param>
        public HasItemsCountMatcher(int expectedCount)
        {
            _expectedCount = expectedCount;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Collection has {_expectedCount} items";

        /// <summary>
        /// Checks if collection contains specified items count.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection contains specific items count; otherwise - false</returns>
        public override bool Matches(ICollection actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (_expectedCount < 0)
            {
                DescribeMismatch("incorrect excpected items count (should be equal or greater than 0)");
                return Reverse;
            }

            int actualCount = actual.Count;

            DescribeMismatch($"having {actualCount} items");

            return actualCount == _expectedCount;
        }
    }
}
