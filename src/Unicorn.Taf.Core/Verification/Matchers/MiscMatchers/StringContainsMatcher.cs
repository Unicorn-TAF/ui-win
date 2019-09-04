namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    public class StringContainsMatcher : TypeSafeMatcher<string>
    {
        private readonly string objectToCompare;

        public StringContainsMatcher(string objectToCompare)
        {
            this.objectToCompare = objectToCompare;
        }

        public override string CheckDescription => $"Contains substring '{this.objectToCompare}'";

        public override bool Matches(string actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
                return Reverse;
            }

            this.DescribeMismatch(actual);
            return actual.Contains(this.objectToCompare);
        }
    }
}
