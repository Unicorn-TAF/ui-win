using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;
using Unicorn.Taf.Core.Verification.Matchers.CoreMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public static class Is
    {
        public static EqualToMatcher<T> EqualTo<T>(T objectToCompare) =>
            new EqualToMatcher<T>(objectToCompare);

        public static NullMatcher Null() =>
            new NullMatcher();

        public static NotMatcher Not(TypeUnsafeMatcher matcher) =>
            new NotMatcher(matcher);

        public static TypeSafeNotMatcher<T> Not<T>(TypeSafeMatcher<T> matcher) =>
            new TypeSafeNotMatcher<T>(matcher);

        public static TypeSafeCollectionNotMatcher<T> Not<T>(TypeSafeCollectionMatcher<T> matcher) =>
            new TypeSafeCollectionNotMatcher<T>(matcher);
    }
}
