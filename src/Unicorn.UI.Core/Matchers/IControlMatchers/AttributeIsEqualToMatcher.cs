using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeIsEqualToMatcher : TypeSafeMatcher<IControl>
    {
        private string attribute, value;

        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"has attribute '{this.attribute}' is equal to '{this.value}'";

        public override bool Matches(IControl actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            string actualValue = actual.GetAttribute(this.attribute);

            DescribeMismatch($"having '{attribute}' = '{actualValue}'");

            return actualValue.Equals(this.value);
        }
    }
}
