using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers
{
    /// <summary>
    /// Matcher to check if collection is equal to specified one. 
    /// </summary>
    /// <typeparam name="T">check items type</typeparam>
    public class IsEqualToCollectionMatcher<T> : TypeSafeCollectionMatcher<T>
    {
        private readonly IEnumerable<T> expectedObjects;
        private string mismatch = string.Empty;

        public IsEqualToCollectionMatcher(IEnumerable<T> expectedObjects)
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

                return "Is equal to collection: [" + itemsList + "]";
            }
        }

        public override bool Matches(IEnumerable<T> actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return this.Reverse;
            }

            bool result;

            if (this.Reverse)
            {
                this.mismatch = "Collections are equal";
                result = this.expectedObjects.Count() != actual.Count();
                result |= actual.Intersect(this.expectedObjects).Count() != this.expectedObjects.Count();
            }
            else
            {
                this.mismatch = "Collections are not equal";
                result = this.expectedObjects.Count() == actual.Count();
                result &= actual.Intersect(this.expectedObjects).Count() == this.expectedObjects.Count();
            }

            if (!result)
            {
                DescribeMismatch(this.mismatch);
            }

            return result;
        }
    }
}
