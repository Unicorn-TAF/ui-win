using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : TypeSafeMatcher<IControl>
    {
        private readonly string attribute;
        private readonly string value;

        public AttributeContainsMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"has attribute '{this.attribute}' contains '{this.value}'";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualValue = actual.GetAttribute(this.attribute);

            DescribeMismatch($"having '{attribute}' = '{actualValue}'");

            return actualValue.Contains(this.value);
        }
    }
}
