using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control attribute value is equal to expected. 
    /// </summary>
    public class AttributeIsEqualToMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _attribute;
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeIsEqualToMatcher"/> class for specified attribute and attribute value.
        /// </summary>
        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            _attribute = attribute;
            _value = value;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has attribute '{_attribute}' is equal to '{_value}'";

        /// <summary>
        /// Checks if UI control attribute value is equal to expected.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control attribute value is equal to expected; otherwise - false</returns>
        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualValue = actual.GetAttribute(_attribute);

            DescribeMismatch($"having '{_attribute}' = '{actualValue}'");

            return actualValue.Equals(_value);
        }
    }
}
