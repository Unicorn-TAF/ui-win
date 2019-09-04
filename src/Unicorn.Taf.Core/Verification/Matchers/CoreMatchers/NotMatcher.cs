namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class NotMatcher : TypeUnsafeMatcher
    {
        private readonly TypeUnsafeMatcher matcher;

        public NotMatcher(TypeUnsafeMatcher matcher)
        {
            matcher.Reverse = true;
            this.matcher = matcher;
        }

        public override string CheckDescription => $"Not {this.matcher.CheckDescription}";

        public override bool Matches(object actual)
        {
            if (this.matcher.Matches(actual))
            {
                this.Output.Append(this.matcher.Output);
                return false;
            }
            
            return true;
        }
    }
}
