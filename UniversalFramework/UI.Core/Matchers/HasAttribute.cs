using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public static class HasAttribute
    {
        public static AttributeContainsMatcher Contains(string attribute, string expectedValue)
        {
            return new AttributeContainsMatcher(attribute, expectedValue);
        }


        public static AttributeIsEqualToMatcher IsEqualTo(string attribute, string expectedValue)
        {
            return new AttributeIsEqualToMatcher(attribute, expectedValue);
        }
    }
}
