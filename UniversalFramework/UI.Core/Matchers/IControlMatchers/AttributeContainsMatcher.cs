using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : TypeSafeMatcher<IControl>
    {
        private string attribute, value;

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

            if (actualValue.Contains(this.value))
            {
                return true;
            }
            else
            {
                DescribeMismatch($"having '{attribute}' = '{actualValue}'");
                return false;
            }
        }
    }
}
