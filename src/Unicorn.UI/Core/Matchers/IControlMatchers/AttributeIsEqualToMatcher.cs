using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeIsEqualToMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _attribute;
        private readonly string _value;

        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            _attribute = attribute;
            _value = value;
        }

        public override string CheckDescription => $"has attribute '{_attribute}' is equal to '{_value}'";

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
