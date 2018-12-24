namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
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

            DescribeMismatch(actual.ToString());
            return actual.Equals(this.objectToCompare);
        }
    }
}
