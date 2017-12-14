using System.Reflection;

namespace Unicorn.Core.Testing.Assertions.Matchers
{
    public class EqualToMatcher : Matcher
    {
        private object objectToCompare;

        public EqualToMatcher(object objectToCompare)
        {
            this.objectToCompare = objectToCompare;
        }

        public override string CheckDescription => "Is equal to " + this.objectToCompare;

        public override bool Matches(object obj)
        {
            return this.IsNotNull(obj) && this.Assertion(obj);
        }

        protected bool Assertion(object obj)
        {
            if (!this.objectToCompare.GetType().Equals(obj.GetType()))
            {
                MatcherOutput.Append($"was not of type {this.objectToCompare.GetType()}");
                return false;
            }
                
            bool isEqual = obj.Equals(this.objectToCompare);

            if (!isEqual)
            {
                DescribeMismatch(obj);
            }

            return isEqual;
        }
    }

    public class NotMatcher : Matcher
    {
        private Matcher matcher;

        public NotMatcher(Matcher matcher)
        {
            FieldInfo partOfNotMatcherField = typeof(Matcher).GetField(
                "partOfNotMatcher",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            this.matcher = matcher;
            this.matcher.MatcherOutput = MatcherOutput;
        }

        public override string CheckDescription => $"Not ({this.matcher.CheckDescription})";

        public override bool Matches(object obj)
        {
            bool result = IsNotNull(obj);

            if (result)
            {
                result = !this.matcher.Matches(obj);

                if (!result)
                {
                    this.matcher.DescribeMismatch(obj);
                }
            }

            MatcherOutput = this.matcher.MatcherOutput;
            return result;
        }
    }
    
    public class IsNullMatcher : Matcher
    {
        public IsNullMatcher() : base()
        {
            this.nullCheckable = false;
        }

        public override string CheckDescription => "Is null";

        public override bool Matches(object obj)
        {
            bool result = obj == null;

            if (!result)
            {
                DescribeMismatch(obj);
            }

            return result;
        }
    }
}
