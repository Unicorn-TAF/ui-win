namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
{
    public class NullMatcher : Matcher
    {
        public NullMatcher() : base()
        {
        }

        public override string CheckDescription => "Is null";

        public override bool Matches(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else
            {
                DescribeMismatch(obj.ToString());
                return false;
            }
        }
    }
}
