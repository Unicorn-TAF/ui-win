using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection has specified item. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class HasItemMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly T _expectedObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasItemMatcher{T}"/> class with specified expected object.
        /// </summary>
        /// <param name="expectedObject">expected item</param>
        public HasItemMatcher(T expectedObject)
        {
            _expectedObject = expectedObject;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"Collection has item {_expectedObject}";

        /// <summary>
        /// Checks if collection contains specified item.
        /// </summary>
        /// <param name="actual">objects collection under check</param>
        /// <returns>true - if collection contains specific item; otherwise - false</returns>
        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(Reverse ? "containing the item" : "not containing the item");

            return actual.Contains(_expectedObject);
        }
    }
}
