using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is equal to specified one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class TheSameAsCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> _expectedObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheSameAsCollectionMatcher{T}"/> class with specified expected collection.
        /// </summary>
        /// <param name="expectedObjects">expected collection</param>
        public TheSameAsCollectionMatcher(IEnumerable<T> expectedObjects)
        {
            _expectedObjects = expectedObjects;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "Is equal to collection: [" + DescribeCollection(_expectedObjects) + "]";

        /// <summary>
        /// Checks if collection is equal to specified one
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection is equal to specified one; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var result = _expectedObjects.Count() == actual.Count();
            result &= actual.Intersect(_expectedObjects).Count() == _expectedObjects.Count();

            DescribeMismatch(DescribeCollection(actual));

            return result;
        }
    }
}
