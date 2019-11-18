using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    /// <summary>
    /// Matcher to check if UI control attribute value contains expected value.
    /// </summary>
    public class AttributeContainsMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _attribute;
        private readonly string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeContainsMatcher"/> class for specified attribute and attribute value part.
        /// </summary>
        public AttributeContainsMatcher(string attribute, string value)
        {
            _attribute = attribute;
            _value = value;
        }

        /// <summary>
        /// Gets check description.
        /// </summary>
        public override string CheckDescription => $"has attribute '{_attribute}' contains '{_value}'";

        /// <summary>
        /// Checks if UI control attribute value contains expected value.
        /// </summary>
        /// <param name="actual">UI control under check</param>
        /// <returns>true - if control attribute value contains expected value; otherwise - false</returns>
        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualValue = actual.GetAttribute(_attribute);

            DescribeMismatch($"having '{_attribute}' = '{actualValue}'");

            return actualValue.Contains(_value);
        }
    }
}
