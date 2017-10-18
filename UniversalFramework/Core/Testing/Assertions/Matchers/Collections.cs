using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class HasItemMatcher : Matcher
    {
        private object ExpectedObject;


        public HasItemMatcher(object expectedObject)
        {
            ExpectedObject = expectedObject;
        }



        public override void DescribeTo()
        {
            Description.Append("Collection has item " + ExpectedObject);
        }


        public override bool Matches(object collection)
        {
            bool result = IsNotNull(collection);

            if (result)
            {
                result = ((IEnumerable<object>)collection).Contains(ExpectedObject);
                if (!result)
                    DescribeMismatch(collection);
            }

            return result;
        }


        public override void DescribeMismatch(object collection)
        {
            //base.DescribeMismatch(collection);
            Description.Append("Collection does not contain the value");
        }
    }


    public class HasItemsMatcher : Matcher
    {
        private IEnumerable<object> ExpectedObjects;
        string mismatch = "";

        public HasItemsMatcher(IEnumerable<object> expectedObjects)
        {
            ExpectedObjects = expectedObjects;
        }


        public override void DescribeTo()
        {
            string itemsList = string.Join(", ", ExpectedObjects);
            if (itemsList.Length > 200)
                itemsList = itemsList.Substring(0, 200) + " etc . . .";
            Description.Append("Collection has items: " + itemsList);
        }


        public override bool Matches(object collection)
        {
            if (!IsNotNull(collection))
                return false;

            bool result;
            IEnumerable<object> _collection = ((IEnumerable<object>)collection);

            if (PartOfNotMatcher)
            {
                result = false;
                mismatch = "Collection contains the value";
                foreach (object obj in ExpectedObjects)
                    result |= _collection.Contains(obj);
            }
            else
            {
                result = true;
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
            //base.DescribeMismatch(collection);
            Description.Append(mismatch);
        }
    }

}
