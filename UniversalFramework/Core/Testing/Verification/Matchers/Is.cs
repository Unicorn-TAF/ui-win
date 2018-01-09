using Unicorn.Core.Testing.Verification.Matchers.CoreMatchers;

namespace Unicorn.Core.Testing.Verification.Matchers
{
    public static class Is
    {
        public static EqualToMatcher EqualTo(object objectToCompare)
        {
            return new EqualToMatcher(objectToCompare);
        }

        public static IsNullMatcher Null()
        {
            return new IsNullMatcher();
        }

        public static NotMatcher Not(Matcher matcher)
        {
            return new NotMatcher(matcher);
        }
    }
}
