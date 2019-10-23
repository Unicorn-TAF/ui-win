using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is sequence equal to another one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class IsSequenceEqualToCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> expected;

        public IsSequenceEqualToCollectionMatcher(IEnumerable<T> expected)
        {
            this.expected = expected;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "Is sequence equal to collection: [" + DescribeCollection(this.expected) + "]";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return this.Reverse;
            }

            DescribeMismatch(DescribeCollection(actual));
            return actual.SequenceEqual(expected);
        }
    }
}
