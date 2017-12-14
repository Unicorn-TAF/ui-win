using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers.CollectionMatchers
{
    public class HasItemsMatcher : Matcher
    {
        private IEnumerable<object> expectedObjects;
        private string mismatch = string.Empty;

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
                return this.partOfNotMatcher;
            }

            bool result = !this.partOfNotMatcher;
            IEnumerable<object> collection = (IEnumerable<object>)collectionObj;

            if (this.partOfNotMatcher)
            {
                this.mismatch = "Collection contains the value";

                foreach (object obj in this.expectedObjects)
                {
                    result |= collection.Contains(obj);
                }
            }
            else
            {
                this.mismatch = "Collection does not contain the value";

                foreach (object obj in this.expectedObjects)
                {
                    result &= collection.Contains(obj);
                }
            }

            if (!result)
            {
                DescribeMismatch(collectionObj);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            this.MatcherOutput.Append(this.mismatch);
        }
    }
}
