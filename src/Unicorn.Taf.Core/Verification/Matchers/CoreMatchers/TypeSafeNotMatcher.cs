namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class TypeSafeNotMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly TypeSafeMatcher<T> matcher;

        public TypeSafeNotMatcher(TypeSafeMatcher<T> matcher)
        {
            matcher.Reverse = true;
            this.matcher = matcher;
        }

        public override string CheckDescription => $"Not {this.matcher.CheckDescription}";

        public override bool Matches(T actual)
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
