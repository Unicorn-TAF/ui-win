using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification
{
    public static class Number
    {
        public static IsEvenMatcher IsEven() =>
            new IsEvenMatcher();

        public static IsPositiveMatcher IsPositive() =>
            new IsPositiveMatcher();
    }
}
