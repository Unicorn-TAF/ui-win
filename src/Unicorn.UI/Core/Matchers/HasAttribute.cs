using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    public class HasAttribute
    {
        private readonly string attribute;

        public HasAttribute(string attribute)
        {
            this.attribute = attribute;
        }

        public static AttributeContainsMatcher HasAttributeContains(string attribute, string expectedValue)
            => new AttributeContainsMatcher(attribute, expectedValue);

        public static AttributeIsEqualToMatcher HasAttributeIsEqualTo(string attribute, string expectedValue)
            => new AttributeIsEqualToMatcher(attribute, expectedValue);

        public AttributeContainsMatcher Contains(string expectedValue)
            => new AttributeContainsMatcher(this.attribute, expectedValue);

        public AttributeIsEqualToMatcher IsEqualTo(string expectedValue)
            => new AttributeIsEqualToMatcher(this.attribute, expectedValue);
    }
}
