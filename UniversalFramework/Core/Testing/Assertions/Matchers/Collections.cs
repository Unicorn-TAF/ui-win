using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class HasItemMatcher : Matcher
    {
        private object expectedObject;
        private string mismatch = string.Empty;

        public HasItemMatcher(object expectedObject)
        {
            this.expectedObject = expectedObject;
        }

        public override string CheckDescription => "Collection has item " + this.expectedObject;

        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
            {
                return this.partOfNotMatcher;
            }

            bool result = ((IEnumerable<object>)collection).Contains(this.expectedObject);

            if (this.partOfNotMatcher)
            {
                mismatch = "Collection contains the value";
            }
            else
            {
                mismatch = "Collection does not contain the value";
            }
            
            if (!result)
            {
                DescribeMismatch(collection);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(mismatch);
        }
    }

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
                DescribeMismatch(collectionObj);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(mismatch);
        }
    }

    public class IsNullOrEmptyMatcher : Matcher
    {
        private string mismatch = string.Empty;

        public IsNullOrEmptyMatcher()
        {
        }

        public override string CheckDescription => "Is empty";

        public override bool Matches(object collectionObj)
        {
            bool result = false;
            ICollection collection = (ICollection)collectionObj;

            if (collection == null)
            {
                result = true;
            }
            else if (collection.Count == 0)
            {
                result = true;
            } 

            if (collection == null)
            {
                mismatch = "was null";
            }
            else
            {
                mismatch = $"had length = {collection.Count}";
            }

            if (!result)
            {
                this.DescribeMismatch(collectionObj);
            }

            return result;
        }

        public override void DescribeMismatch(object collection)
        {
            this.MatcherOutput.Append(mismatch);
        }
    }

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
                return this.partOfNotMatcher;
            }

            bool result = !this.partOfNotMatcher;
            IEnumerable<object> collection = (IEnumerable<object>)collectionObj;

            if (this.partOfNotMatcher)
            {
                mismatch = "Collections are equal";
                result = this.expectedObjects.Count() != collection.Count();
                result |= collection.Intersect(this.expectedObjects).Count() != this.expectedObjects.Count();
            }
            else
            {
                mismatch = "Collections are not equal";
                result = this.expectedObjects.Count() == collection.Count();
                result &= collection.Intersect(this.expectedObjects).Count() == this.expectedObjects.Count();
            }

            if (!result)
            {
                DescribeMismatch(mismatch);
            }

            return result;
        }
    }
}
