namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
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
                DescribeMismatch($"not of type {this.objectToCompare.GetType()}");
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
}
