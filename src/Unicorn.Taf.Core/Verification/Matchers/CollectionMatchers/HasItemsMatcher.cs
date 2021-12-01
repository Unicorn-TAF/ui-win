using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection has specified items array. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class HasItemsMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> _expectedObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasItemsMatcher{T}"/> class with specified expected objects.
        /// </summary>
        /// <param name="expectedObjects">expected objects</param>
        public HasItemsMatcher(IEnumerable<T> expectedObjects)
        {
            _expectedObjects = expectedObjects;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription =>
            "Collection has items: " + DescribeCollection(_expectedObjects, 200);

        /// <summary>
        /// Checks if collection contains specified items.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection contains specific items; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var mismatchItems = Reverse ?
                actual.Where(i => _expectedObjects.Contains(i)) :
                _expectedObjects.Where(i => !actual.Contains(i));

            DescribeMismatch($"items {(Reverse ? "" : "not ")}presented: {string.Join(", ", mismatchItems)}");

            return mismatchItems.Any() ? Reverse : !Reverse;
        }
    }
}
