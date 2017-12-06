using Unicorn.Core.Testing.Assertions;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Matchers.IControlMatchers
{
    public class AttributeContainsMatcher : Matcher
    {
        private string Attribute, Value;

        public override string CheckDescription
        {
            get
            {
                return $"Attribute '{Attribute}' contains '{Value}'";
            }
        }


        public AttributeContainsMatcher(string attribute, string value)
        {
            Attribute = attribute;
            Value = value;
        }


        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && Assertion(_object);
        }


        protected bool Assertion(object _object)
        {
            if(!_object.GetType().IsSubclassOf(typeof(IControl)))
            {
                MatcherOutput.Append($"was not an instance of IControl");
                return false;
            }

            IControl element = (IControl)_object;
            string actualValue = element.GetAttribute(Attribute);

            bool contains = actualValue.Contains(Value);
            if (!contains)
                MatcherOutput.Append("was ").Append(actualValue);

            return contains;
        }
    }


    public class AttributeIsEqualToMatcher : Matcher
    {
        private string Attribute, Value;

        public override string CheckDescription { get { return $"Attribute '{Attribute}' is equal to '{Value}'"; } }

        public AttributeIsEqualToMatcher(string attribute, string value)
        {
            Attribute = attribute;
            Value = value;
        }


        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && Assertion(_object);
        }


        protected bool Assertion(object _object)
        {
            if (!_object.GetType().IsSubclassOf(typeof(IControl)))
            {
                MatcherOutput.Append($"was not an instance of IControl");
                return false;
            }

            IControl element = (IControl)_object;
            string actualValue = element.GetAttribute(Attribute);

            bool equals = actualValue.Equals(Value);
            if (!equals)
                MatcherOutput.Append("was ").Append(actualValue);

            return equals;
        }
    }
}
