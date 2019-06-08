namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class NullMatcher : TypeUnsafeMatcher
    {
        public NullMatcher() : base()
        {
        }

        public override string CheckDescription => "Is null";

        public override bool Matches(object actual)
        {
            if (actual == null)
            {
                DescribeMismatch("null");
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
