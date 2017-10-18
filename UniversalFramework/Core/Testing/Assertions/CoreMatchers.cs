using System.Collections.Generic;
using System.Reflection;
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
}
