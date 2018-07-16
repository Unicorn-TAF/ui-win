using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public class HasAttribute
    {
        private string attribute;

        public HasAttribute(string attribute)
        {
            this.attribute = attribute;
        }

        public static AttributeContainsMatcher Contains(string attribute, string expectedValue)
        {
            return new AttributeContainsMatcher(attribute, expectedValue);
        }

        public static AttributeIsEqualToMatcher IsEqualTo(string attribute, string expectedValue)
        {
            return new AttributeIsEqualToMatcher(attribute, expectedValue);
        }

        public AttributeContainsMatcher Contains(string expectedValue)
        {
            return new AttributeContainsMatcher(this.attribute, expectedValue);
        }

        public AttributeIsEqualToMatcher IsEqualTo(string expectedValue)
        {
            return new AttributeIsEqualToMatcher(this.attribute, expectedValue);
        }
    }
}
