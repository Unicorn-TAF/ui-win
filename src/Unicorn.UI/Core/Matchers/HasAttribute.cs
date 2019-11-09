using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for attribute matchers.
    /// </summary>
    public class HasAttribute
    {
        private readonly string _attribute;

        /// <summary>
        /// Initializes a new instance of the <see cref="HasAttribute"/> class with specified attribute name.
        /// </summary>
        /// <param name="attribute"></param>
        public HasAttribute(string attribute)
        {
            _attribute = attribute;
        }

        public static AttributeContainsMatcher HasAttributeContains(string attribute, string expectedValue)
            => new AttributeContainsMatcher(attribute, expectedValue);

        public static AttributeIsEqualToMatcher HasAttributeIsEqualTo(string attribute, string expectedValue)
            => new AttributeIsEqualToMatcher(attribute, expectedValue);

        public AttributeContainsMatcher Contains(string expectedValue)
            => new AttributeContainsMatcher(_attribute, expectedValue);

        public AttributeIsEqualToMatcher IsEqualTo(string expectedValue)
            => new AttributeIsEqualToMatcher(_attribute, expectedValue);
    }
}
