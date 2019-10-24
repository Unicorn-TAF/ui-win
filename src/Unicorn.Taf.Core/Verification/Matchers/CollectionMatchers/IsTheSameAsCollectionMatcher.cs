using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is equal to specified one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class IsTheSameAsCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> expectedObjects;

        public IsTheSameAsCollectionMatcher(IEnumerable<T> expectedObjects)
        {
            this.expectedObjects = expectedObjects;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "Is equal to collection: [" + DescribeCollection(this.expectedObjects) + "]";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return this.Reverse;
            }

            var result = this.expectedObjects.Count() == actual.Count();
            result &= actual.Intersect(this.expectedObjects).Count() == this.expectedObjects.Count();

            DescribeMismatch(DescribeCollection(actual));

            return result;
        }
    }
}
