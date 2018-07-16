using Unicorn.Core.Testing.Verification.Matchers.MiscMatchers;

namespace Unicorn.Core.Testing.Verification
{
    public class MiscMatchers
    {
        public static IsEvenMatcher IsEven()
        {
            return new IsEvenMatcher();
        }

        public static StringContainsMatcher StringContains(string objectToCompare)
        {
            return new StringContainsMatcher(objectToCompare);
        }
    }
}
