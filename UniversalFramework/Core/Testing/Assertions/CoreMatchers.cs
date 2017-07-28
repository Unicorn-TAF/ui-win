using System.Collections.Generic;
using Unicorn.Core.Testing.Assertions.Matchers;

namespace Unicorn.Core.Testing.Assertions
{

    public static class CoreMatchers
    {
        public static EqualToMatcher IsEqualTo(object objectToCompare)
        {
            return new EqualToMatcher(objectToCompare);
        }
        

        public static IsNullMatcher IsNull()
        {
            return new IsNullMatcher();
        }


        public static NotMatcher Not(Matcher matcher)
        {
            return new NotMatcher(matcher);
        }


        public static IsEvenMatcher IsEven()
        {
            return new IsEvenMatcher();
        }
    }



    public static class CollectionsMatchers
    {
        public static HasItemMatcher HasItem(object expectedObject)
        {
            return new HasItemMatcher(expectedObject);
        }

        public static HasItemsMatcher HasItems(IEnumerable<object> expectedObjects)
        {
            return new HasItemsMatcher(expectedObjects);
        }
    }


    

    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override void DescribeTo()
        {
            Description.Append("An Even number");
        }

        protected override bool Assertion(object number)
        {
            bool isEven = (int)number % 2 == 0;
            if (!isEven)
                DescribeMismatch(number);

            return isEven;
        }

        public override void DescribeMismatch(object number)
        {
            base.DescribeMismatch(number);
            //Description.Append(", which is an Odd number");
        }
    }
}
