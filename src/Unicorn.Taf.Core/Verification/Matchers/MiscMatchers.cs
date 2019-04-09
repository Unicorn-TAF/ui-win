using Unicorn.Taf.Core.Verification.Matchers.MiscMatchers;

namespace Unicorn.Taf.Core.Verification
{
    public static class MiscMatchers
    {
        public static IsEvenMatcher IsEven() =>
            new IsEvenMatcher();

        public static StringContainsMatcher StringContains(string objectToCompare) =>
            new StringContainsMatcher(objectToCompare);
    }
}
