using Unicorn.Core.Testing.Assertions.Matchers;

namespace Unicorn.Core.Testing.Assertions
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
