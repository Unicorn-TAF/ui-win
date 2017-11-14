using System.Reflection;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class EqualToMatcher : Matcher
    {
        private object ObjectToCompare;

        public override string CheckDescription
        {
            get
            {
                return "Is equal to " + ObjectToCompare;
            }
        }


        public EqualToMatcher(object objectToCompare)
        {
            ObjectToCompare = objectToCompare;
        }


        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && Assertion(_object);
        }


        protected bool Assertion(object _object)
        {
            if (!ObjectToCompare.GetType().Equals(_object.GetType()))
            {
                MatcherOutput.Append($"was not of type {ObjectToCompare.GetType()}");
                return false;
            }
                
            bool isEqual = _object.Equals(ObjectToCompare);
            if (!isEqual)
                DescribeMismatch(_object);

            return isEqual;
        }
    }


    public class NotMatcher : Matcher
    {
        private Matcher _matcher;

        public override string CheckDescription
        {
            get
            {
                return $"Not ({_matcher.CheckDescription})";
            }
        }


        public NotMatcher(Matcher matcher)
        {
            FieldInfo partOfNotMatcherField = typeof(Matcher).GetField("PartOfNotMatcher",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            _matcher = matcher;
            _matcher.MatcherOutput = MatcherOutput;
        }


        public override bool Matches(object _object)
        {
            bool result = IsNotNull(_object);

            if (result)
            {
                result = !_matcher.Matches(_object);
                if (!result)
                    _matcher.DescribeMismatch(_object);
            }

            MatcherOutput = _matcher.MatcherOutput;
            return result;
        }
    }
    

    public class IsNullMatcher : Matcher
    {
        public override string CheckDescription
        {
            get
            {
                return "Is null";
            }
        }


        public IsNullMatcher() : base()
        {
            NullCheckable = false;
        }


        public override bool Matches(object _object)
        {
            bool result = _object == null;

            if (!result)
                DescribeMismatch(_object);

            return result;
        }
    }
}
