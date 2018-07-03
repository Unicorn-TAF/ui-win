using Unicorn.Core.Testing.Verification.Matchers.CoreMatchers;

namespace Unicorn.Core.Testing.Verification.Matchers
{
    public static class Is
    {
        public static EqualToMatcher EqualTo(object objectToCompare)
        {
            return new EqualToMatcher(objectToCompare);
        }

        public static NullMatcher Null()
        {
            return new NullMatcher();
        }

        public static NotMatcher Not(Matcher matcher)
        {
            return new NotMatcher(matcher);
        }
    }
}
