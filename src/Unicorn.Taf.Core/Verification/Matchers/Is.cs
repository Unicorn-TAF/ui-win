using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;
using Unicorn.Taf.Core.Verification.Matchers.CoreMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    /// <summary>
    /// Entry point for core matchers.
    /// </summary>
    public static class Is
    {
        /// <summary>
        /// Matcher to check if actual object is equal to expected one.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="objectToCompare">expected item to check equality</param>
        /// <returns><see cref="EqualToMatcher{T}"/> instance</returns>
        public static EqualToMatcher<T> EqualTo<T>(T objectToCompare) =>
            new EqualToMatcher<T>(objectToCompare);

        /// <summary>
        /// Matcher to check if object is null.
        /// </summary>
        /// <returns><see cref="NullMatcher"/> instance</returns>
        public static NullMatcher Null() =>
            new NullMatcher();

        /// <summary>
        /// Matcher to negotiate action of another matcher.
        /// </summary>
        /// <param name="matcher">instance of matcher with specified check</param>
        /// <returns><see cref="NotMatcher"/> instance</returns>
        public static NotMatcher Not(TypeUnsafeMatcher matcher) =>
            new NotMatcher(matcher);

        /// <summary>
        /// Matcher to negotiate action of another matcher.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="matcher">instance of matcher with specified check</param>
        /// <returns><see cref="TypeSafeNotMatcher{T}"/> instance</returns>
        public static TypeSafeNotMatcher<T> Not<T>(TypeSafeMatcher<T> matcher) =>
            new TypeSafeNotMatcher<T>(matcher);

        /// <summary>
        /// Matcher to negotiate action of specified collection matcher.
        /// </summary>
        /// <typeparam name="T">check items type</typeparam>
        /// <param name="matcher">instance of collection matcher with specified check</param>
        /// <returns><see cref="TypeSafeCollectionNotMatcher{T}"/> instance</returns>
        public static TypeSafeCollectionNotMatcher<T> Not<T>(TypeSafeCollectionMatcher<T> matcher) =>
            new TypeSafeCollectionNotMatcher<T>(matcher);
    }
}
