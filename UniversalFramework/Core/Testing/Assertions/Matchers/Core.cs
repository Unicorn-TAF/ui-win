namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class EqualToMatcher : Matcher
    {
        private object ObjectToCompare;


        public EqualToMatcher(object objectToCompare)
        {
            ObjectToCompare = objectToCompare;
        }



        public override void DescribeTo()
        {
            Description.Append("Is equal to " + ObjectToCompare);
        }


        public override bool Matches(object _object)
        {
            return IsNotNull(_object) && Assertion(_object);
        }


        protected bool Assertion(object _object)
        {
            if (!ObjectToCompare.GetType().Equals(_object.GetType()))
            {
                Description.Append($"was not of type {ObjectToCompare.GetType()}");
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


        public NotMatcher(Matcher matcher)
        {
            _matcher = matcher;
            _matcher.Description = Description;
        }



        public override void DescribeTo()
        {
            _matcher.Description.Append("Not ");
            _matcher.DescribeTo();
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

            Description = _matcher.Description;
            return result;
        }
    }
    

    public class IsNullMatcher : Matcher
    {
        public IsNullMatcher() : base()
        {
            NullCheckable = false;
        }



        public override void DescribeTo()
        {
            Description.Append("Is null");
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
