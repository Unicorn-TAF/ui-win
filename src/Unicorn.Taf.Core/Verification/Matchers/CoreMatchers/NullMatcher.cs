namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class NullMatcher : Matcher
    {
        public NullMatcher() : base()
        {
        }

        public override string CheckDescription => "Is null";

        public override bool Matches(object actual)
        {
            if (actual == null)
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
