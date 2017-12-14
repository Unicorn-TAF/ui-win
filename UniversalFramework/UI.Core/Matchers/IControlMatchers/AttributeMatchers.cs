using Unicorn.Core.Testing.Assertions;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : Matcher
    {
        private string attribute, value;

        public AttributeContainsMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"Attribute '{this.attribute}' contains '{this.value}'";

        public override bool Matches(object obj)
        {
            return this.IsNotNull(obj) && this.Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(IControl)))
            {
                MatcherOutput.Append($"was not an instance of IControl");
                return false;
            }

            IControl element = (IControl)obj;
            string actualValue = element.GetAttribute(this.attribute);

            bool contains = actualValue.Contains(this.value);

            if (!contains)
            {
                this.MatcherOutput.Append("was ").Append(actualValue);
            }
                
            return contains;
        }
    }

    public class AttributeIsEqualToMatcher : Matcher
    {
        private string attribute, value;

        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            this.attribute = attribute;
            this.value = value;
        }

        public override string CheckDescription => $"Attribute '{this.attribute}' is equal to '{this.value}'";

        public override bool Matches(object obj)
        {
            return this.IsNotNull(obj) && this.Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            if (!obj.GetType().IsSubclassOf(typeof(IControl)))
            {
                MatcherOutput.Append($"was not an instance of IControl");
                return false;
            }

            IControl element = (IControl)obj;
            string actualValue = element.GetAttribute(this.attribute);

            bool equals = actualValue.Equals(value);

            if (!equals)
            {
                this.MatcherOutput.Append("was ").Append(actualValue);
            }

            return equals;
        }
    }
}
