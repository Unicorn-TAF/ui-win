using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is sequence equal to another one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class SequenceEqualToCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> _expected;

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceEqualToCollectionMatcher{T}"/> class with specified expected collection.
        /// </summary>
        /// <param name="expected">expected collection</param>
        public SequenceEqualToCollectionMatcher(IEnumerable<T> expected)
        {
            _expected = expected;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "Is sequence equal to collection: [" + DescribeCollection(_expected, 200) + "]";

        /// <summary>
        /// Checks if collection is sequence equal to another one
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection is sequence equal to another one; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(DescribeCollection(actual, 1000));
            return actual.SequenceEqual(_expected);
        }
    }
}
