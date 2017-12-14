namespace Unicorn.Core.Testing.Assertions.Matchers.CoreMatchers
{
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
