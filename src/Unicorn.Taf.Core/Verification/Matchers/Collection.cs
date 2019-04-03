using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public static class Collection
    {
        public static IsNullOrEmptyMatcher IsNullOrEmpty()
        {
            return new IsNullOrEmptyMatcher();
        }

        public static HasItemMatcher HasItem(object expectedObject)
        {
            return new HasItemMatcher(expectedObject);
        }

        public static HasItemsMatcher HasItems(IEnumerable<object> expectedObjects)
        {
            return new HasItemsMatcher(expectedObjects);
        }

        public static IsEqualToCollectionMatcher IsEqualToCollection(IEnumerable<object> expectedObjects)
        {
            return new IsEqualToCollectionMatcher(expectedObjects);
        }
    }
}
