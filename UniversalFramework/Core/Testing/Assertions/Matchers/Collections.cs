using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class HasItemMatcher : Matcher
    {
        private object ExpectedObject;
        string mismatch = "";

        public override string CheckDescription
        {
            get
            {
                return "Collection has item " + ExpectedObject;
            }
        }


        public HasItemMatcher(object expectedObject)
        {
            ExpectedObject = expectedObject;
        }


        public override bool Matches(object collection)
        {
            if(!IsNotNull(collection))
                return PartOfNotMatcher;

            bool result = ((IEnumerable<object>)collection).Contains(ExpectedObject);

            if (PartOfNotMatcher)
                mismatch = "Collection contains the value";
            else
                mismatch = "Collection does not contain the value";
            
            if (!result)
                DescribeMismatch(collection);

            return result;
        }


        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(mismatch);
        }
    }


    public class HasItemsMatcher : Matcher
    {
        private IEnumerable<object> ExpectedObjects;
        string mismatch = "";

        public override string CheckDescription
        {
            get
            {
                string itemsList = string.Join(", ", ExpectedObjects);
                if (itemsList.Length > 200)
                    itemsList = itemsList.Substring(0, 200) + " etc . . .";

                return "Collection has items: " + itemsList;
            }
        }

        public HasItemsMatcher(IEnumerable<object> expectedObjects)
        {
            ExpectedObjects = expectedObjects;
        }


        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
                return PartOfNotMatcher;

            bool result = !PartOfNotMatcher;
            IEnumerable<object> _collection = ((IEnumerable<object>)collection);

            if (PartOfNotMatcher)
            {
                mismatch = "Collection contains the value";
                foreach (object obj in ExpectedObjects)
                    result |= _collection.Contains(obj);
            }
            else
            {
                mismatch = "Collection does not contain the value";
                foreach (object obj in ExpectedObjects)
                    result &= _collection.Contains(obj);
            }

            if (!result)
                DescribeMismatch(collection);

            return result;
        }


        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(mismatch);
        }
    }


    public class IsNullOrEmptyMatcher : Matcher
    {
        string mismatch = "";

        public override string CheckDescription
        {
            get
            {
                return "Is empty";
            }
        }

        public IsNullOrEmptyMatcher()
        {
        }


        public override bool Matches(object collection)
        {
            bool result = false;
            ICollection _collection = ((ICollection)collection);

            if (_collection == null)
                result = true;
            else if(_collection.Count == 0)
                result = true;

            if (_collection == null)
                mismatch = "was null";
            else
                mismatch = $"had length = {_collection.Count}";

            if (!result)
                DescribeMismatch(collection);

            return result;
        }


        public override void DescribeMismatch(object collection)
        {
            MatcherOutput.Append(mismatch);
        }
    }


    public class IsEqualToCollectionMatcher : Matcher
    {
        private IEnumerable<object> ExpectedObjects;
        string mismatch = "";

        public override string CheckDescription
        {
            get
            {
                string itemsList = string.Join(", ", ExpectedObjects);
                if (itemsList.Length > 200)
                    itemsList = itemsList.Substring(0, 200) + " etc . . .";

                return "Is equal to collection: [" + itemsList + "]";
            }
        }

        public IsEqualToCollectionMatcher(IEnumerable<object> expectedObjects)
        {
            ExpectedObjects = expectedObjects;
        }


        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
                return PartOfNotMatcher;

            bool result = !PartOfNotMatcher;
            IEnumerable<object> _collection = ((IEnumerable<object>)collection);
            

            if (PartOfNotMatcher)
            {
                mismatch = "Collections are equal";
                result = ExpectedObjects.Count() != _collection.Count();
                result |= _collection.Intersect(ExpectedObjects).Count() != ExpectedObjects.Count();
            }
            else
            {
                mismatch = "Collections are not equal";
                result = ExpectedObjects.Count() == _collection.Count();
                result &= _collection.Intersect(ExpectedObjects).Count() == ExpectedObjects.Count();
            }

            if (!result)
                DescribeMismatch(collection);

            return result;
        }
    }

}
