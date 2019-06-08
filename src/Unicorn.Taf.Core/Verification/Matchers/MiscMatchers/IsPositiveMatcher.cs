namespace Unicorn.Taf.Core.Verification.Matchers.MiscMatchers
{
    public class IsPositiveMatcher : TypeSafeMatcher<int>
    {
        public override string CheckDescription => "Is positive number";

        public override bool Matches(int actual)
        {
            DescribeMismatch(actual.ToString());
            return actual > 0;
        }
    }
}
