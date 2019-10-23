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
        private readonly IEnumerable<T> expectedObjects;
        
        public HasItemsMatcher(IEnumerable<T> expectedObjects)
        {
            this.expectedObjects = expectedObjects;
        }

        /// <summary>
        /// Gets check description
        /// </summary>
        public override string CheckDescription
        {
            get
            {
                string itemsList = string.Join(", ", this.expectedObjects);

                if (itemsList.Length > 200)
                {
                    itemsList = itemsList.Substring(0, 200) + " etc . . .";
                }

                return "Collection has items: " + itemsList;
            }
        }

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            var mismatchItems = this.Reverse ?
                actual.Where(i => expectedObjects.Contains(i)) :
                expectedObjects.Where(i => !actual.Contains(i));

            DescribeMismatch($"items {(this.Reverse ? "" : "not ")}presented: {string.Join(", ", mismatchItems)}");

            return mismatchItems.Any() ? Reverse : !Reverse;
        }
    }
}
