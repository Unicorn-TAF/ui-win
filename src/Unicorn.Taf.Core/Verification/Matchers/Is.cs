using Unicorn.Taf.Core.Verification.Matchers.CoreMatchers;

namespace Unicorn.Taf.Core.Verification.Matchers
{
    public static class Is
    {
        public static EqualToMatcher<T> EqualTo<T>(T objectToCompare)
        {
            return new EqualToMatcher<T>(objectToCompare);
        }

        public static NullMatcher Null()
        {
            return new NullMatcher();
        }

        public static NotMatcher Not(Matcher matcher)
        {
            return new NotMatcher(matcher);
        }

        public static TypeSafeNotMatcher<T> Not<T>(TypeSafeMatcher<T> matcher)
        {
            return new TypeSafeNotMatcher<T>(matcher);
        }
    }
}
