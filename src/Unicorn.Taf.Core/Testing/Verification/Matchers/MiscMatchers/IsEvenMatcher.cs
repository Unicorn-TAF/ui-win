namespace Unicorn.Taf.Core.Testing.Verification.Matchers.MiscMatchers
{
    public class IsEvenMatcher : TypeSafeMatcher<int>
    {
        public override string CheckDescription => "An Even number";

        public override bool Matches(int actual)
        {
            if (actual % 2 == 0)
            {
                return true;
            }
            else
            {
                DescribeMismatch(actual.ToString());
                return false;
            }
        }
    }
}
