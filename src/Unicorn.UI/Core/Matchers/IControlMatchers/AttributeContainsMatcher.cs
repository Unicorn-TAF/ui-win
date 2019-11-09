using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string _attribute;
        private readonly string _value;

        public AttributeContainsMatcher(string attribute, string value)
        {
            _attribute = attribute;
            _value = value;
        }

        public override string CheckDescription => $"has attribute '{_attribute}' contains '{_value}'";

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
