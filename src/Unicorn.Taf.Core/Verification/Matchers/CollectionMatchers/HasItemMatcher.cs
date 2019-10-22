using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    public class HasItemMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly T expectedObject;

        public HasItemMatcher(T expectedObject)
        {
            this.expectedObject = expectedObject;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription => $"Collection has item {this.expectedObject}";

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            DescribeMismatch(this.Reverse ? "containing the item" : "not containing the item");

            return actual.Contains(this.expectedObject);
        }
    }
}
