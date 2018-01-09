using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Verification.Matchers.CollectionMatchers
{
    public class HasItemsMatcher : Matcher
    {
        private IEnumerable<object> expectedObjects;
        
        public HasItemsMatcher(IEnumerable<object> expectedObjects)
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

                return "Collection has items: " + itemsList;
            }
        }

        public override bool Matches(object collectionObj)
        {
            if (!IsNotNull(collectionObj))
            {
                return this.Reverse;
            }

            string mismatch = string.Empty;
            bool result = !this.Reverse;
            IEnumerable<object> collection = (IEnumerable<object>)collectionObj;

            if (this.Reverse)
            {
                mismatch = "Collection contains the value";

                foreach (object obj in this.expectedObjects)
                {
                    result |= collection.Contains(obj);
                }
            }
            else
            {
                mismatch = "Collection does not contain the value";

                foreach (object obj in this.expectedObjects)
                {
                    result &= collection.Contains(obj);
                }
            }

            if (!result)
            {
                DescribeMismatch(mismatch);
            }

            return result;
        }
    }
}
