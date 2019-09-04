using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public static class Collection
    {
        public static IsNullOrEmptyMatcher IsNullOrEmpty() =>
            new IsNullOrEmptyMatcher();

        public static HasItemMatcher HasItem(object expectedObject) =>
            new HasItemMatcher(expectedObject);

        public static HasItemsMatcher HasItems(IEnumerable<object> expectedObjects) =>
            new HasItemsMatcher(expectedObjects);

        public static IsEqualToCollectionMatcher IsEqualToCollection(IEnumerable<object> expectedObjects) =>
            new IsEqualToCollectionMatcher(expectedObjects);

        public static EachMatcher<T> Each<T>(TypeSafeMatcher<T> matcher) =>
            new EachMatcher<T>(matcher);

        public static AnyMatcher<T> Any<T>(TypeSafeMatcher<T> matcher) =>
            new AnyMatcher<T>(matcher);
    }
}
