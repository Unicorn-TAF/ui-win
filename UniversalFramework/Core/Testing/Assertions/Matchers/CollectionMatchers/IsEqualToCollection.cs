using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers.CollectionMatchers
{
    public class IsEqualToCollectionMatcher : Matcher
    {
        private IEnumerable<object> expectedObjects;
        private string mismatch = string.Empty;

        public IsEqualToCollectionMatcher(IEnumerable<object> expectedObjects)
        {
            this.expectedObjects = expectedObjects;
        }

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

        public override bool Matches(object collectionObj)
        {
            if (!IsNotNull(collectionObj))
            {
                return this.Reverse;
            }

            bool result = !this.Reverse;
            IEnumerable<object> collection = (IEnumerable<object>)collectionObj;

            if (this.Reverse)
            {
                this.mismatch = "Collections are equal";
                result = this.expectedObjects.Count() != collection.Count();
                result |= collection.Intersect(this.expectedObjects).Count() != this.expectedObjects.Count();
            }
            else
            {
                this.mismatch = "Collections are not equal";
                result = this.expectedObjects.Count() == collection.Count();
                result &= collection.Intersect(this.expectedObjects).Count() == this.expectedObjects.Count();
            }

            if (!result)
            {
                DescribeMismatch(this.mismatch);
            }

            return result;
        }
    }
}
