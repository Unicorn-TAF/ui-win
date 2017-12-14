using Unicorn.Core.Testing.Assertions.Matchers.CoreMatchers;

namespace Unicorn.Core.Testing.Assertions.Matchers
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
