using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public static class Collection
    {
        public static IsNullOrEmptyMatcher IsNullOrEmpty() =>
            new IsNullOrEmptyMatcher();

        public static HasItemMatcher<T> HasItem<T>(T expectedObject) =>
            new HasItemMatcher<T>(expectedObject);

        public static HasItemsMatcher<T> HasItems<T>(IEnumerable<T> expectedObjects) =>
            new HasItemsMatcher<T>(expectedObjects);

        public static IsEqualToCollectionMatcher<T> IsEqualTo<T>(IEnumerable<T> expectedObjects) =>
            new IsEqualToCollectionMatcher<T>(expectedObjects);

        public static IsSequenceEqualToCollectionMatcher<T> IsSequenceEqualTo<T>(IEnumerable<T> expectedObjects) =>
            new IsSequenceEqualToCollectionMatcher<T>(expectedObjects);

        public static EachMatcher<T> Each<T>(TypeSafeMatcher<T> matcher) =>
            new EachMatcher<T>(matcher);

        public static AnyMatcher<T> Any<T>(TypeSafeMatcher<T> matcher) =>
            new AnyMatcher<T>(matcher);
    }
}
