namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class EqualToMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly T objectToCompare;

        public EqualToMatcher(T objectToCompare)
        {
            this.objectToCompare = objectToCompare;
        }

        public override string CheckDescription => "Is equal to " + this.objectToCompare;

        public override bool Matches(T actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            if (!this.objectToCompare.GetType().Equals(actual.GetType()))
            {
                DescribeMismatch($"not of type {this.objectToCompare.GetType()}");
                return false;
            }

            DescribeMismatch(actual.ToString());
            return actual.Equals(this.objectToCompare);
        }
    }
}
