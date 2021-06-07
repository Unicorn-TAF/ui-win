using System;
using Unicorn.UI.Core.Matchers.IControlMatchers;

namespace Unicorn.UI.Core.Matchers
{
    /// <summary>
    /// Entry point for attribute matchers.
    /// </summary>
    [Obsolete("Please use Unicorn.UI.Core.Matchers.Ui entry point")]
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

        /// <summary>
        /// Matcher to check if UI control has specified attribute which contains expected value.
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <param name="expectedValue">expected part of attribute value</param>
        /// <returns><see cref="AttributeContainsMatcher"/> instance</returns>
        public static AttributeContainsMatcher HasAttributeContains(string attribute, string expectedValue)
            => new AttributeContainsMatcher(attribute, expectedValue);

        /// <summary>
        /// Matcher to check if UI control has specified attribute with specified value.
        /// </summary>
        /// <param name="attribute">attribute name</param>
        /// <param name="expectedValue">expected attribute value</param>
        /// <returns><see cref="AttributeIsEqualToMatcher"/> instance</returns>
        public static AttributeIsEqualToMatcher HasAttributeIsEqualTo(string attribute, string expectedValue)
            => new AttributeIsEqualToMatcher(attribute, expectedValue);

        /// <summary>
        /// Matcher to check if UI control has specified attribute which contains expected value.
        /// </summary>
        /// <param name="expectedValue">expected part of attribute value</param>
        /// <returns><see cref="AttributeContainsMatcher"/> instance</returns>
        public AttributeContainsMatcher Contains(string expectedValue)
            => new AttributeContainsMatcher(_attribute, expectedValue);

        /// <summary>
        /// Matcher to check if UI control has specified attribute with specified value.
        /// </summary>
        /// <param name="expectedValue">expected attribute value</param>
        /// <returns><see cref="AttributeIsEqualToMatcher"/> instance</returns>
        public AttributeIsEqualToMatcher IsEqualTo(string expectedValue)
            => new AttributeIsEqualToMatcher(_attribute, expectedValue);
    }
}
