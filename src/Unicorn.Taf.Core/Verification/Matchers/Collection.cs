using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Entry point for collections matchers.
    /// </summary>
    public static class Collection
    {
        /// <summary>
        /// Matcher to check if collection is null or empty.
        /// </summary>
        /// <returns><see cref="IsNullOrEmptyMatcher"/> instance</returns>
        public static IsNullOrEmptyMatcher IsNullOrEmpty() =>
            new IsNullOrEmptyMatcher();

        /// <summary>
        /// Matcher to check if collection contains specified item.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="expectedObject">item expected in collection</param>
        /// <returns><see cref="HasItemMatcher{T}"/> instance</returns>
        public static HasItemMatcher<T> HasItem<T>(T expectedObject) =>
            new HasItemMatcher<T>(expectedObject);

        /// <summary>
        /// Matcher to check if collection contains specified items.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="expectedObjects">items expected in collection</param>
        /// <returns><see cref="HasItemsMatcher{T}"/> instance</returns>
        public static HasItemsMatcher<T> HasItems<T>(IEnumerable<T> expectedObjects) =>
            new HasItemsMatcher<T>(expectedObjects);

        /// <summary>
        /// Matcher to check if collection contains specified items count.
        /// </summary>
        /// <param name="expectedCount">items count expected in collection</param>
        /// <returns><see cref="HasItemsCountMatcher"/> instance</returns>
        public static HasItemsCountMatcher HasItemsCount(int expectedCount) =>
            new HasItemsCountMatcher(expectedCount);

        /// <summary>
        /// Matcher to check if collection has the same items as expected one ignoring order.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="expectedObjects">expected collection</param>
        /// <returns><see cref="TheSameAsCollectionMatcher{T}"/> instance</returns>
        public static TheSameAsCollectionMatcher<T> IsTheSameAs<T>(IEnumerable<T> expectedObjects) =>
            new TheSameAsCollectionMatcher<T>(expectedObjects);

        /// <summary>
        /// Matcher to check if collection is sequence equal to expected.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="expectedObjects">expected collection</param>
        /// <returns><see cref="SequenceEqualToCollectionMatcher{T}"/> instance</returns>
        public static SequenceEqualToCollectionMatcher<T> IsSequenceEqualTo<T>(IEnumerable<T> expectedObjects) =>
            new SequenceEqualToCollectionMatcher<T>(expectedObjects);

        /// <summary>
        /// Matcher to check if each collection item satisfies specified matcher.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="matcher">matcher to check against each item</param>
        /// <returns><see cref="EachMatcher{T}"/> instance</returns>
        public static EachMatcher<T> Each<T>(TypeSafeMatcher<T> matcher) =>
            new EachMatcher<T>(matcher);

        /// <summary>
        /// Matcher to check if at least one collection item satisfies specified matcher.
        /// </summary>
        /// <typeparam name="T">items type</typeparam>
        /// <param name="matcher">matcher to check against collection items</param>
        /// <returns><see cref="AnyMatcher{T}"/> instance</returns>
        public static AnyMatcher<T> Any<T>(TypeSafeMatcher<T> matcher) =>
            new AnyMatcher<T>(matcher);
    }
}
