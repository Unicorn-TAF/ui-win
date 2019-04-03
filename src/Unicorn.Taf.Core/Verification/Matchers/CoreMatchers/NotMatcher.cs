using System.Reflection;

namespace Unicorn.Taf.Core.Verification.Matchers.CoreMatchers
{
    public class NotMatcher : Matcher
    {
        private readonly Matcher matcher;

        public NotMatcher(Matcher matcher)
        {
            PropertyInfo partOfNotMatcherField = typeof(Matcher).GetProperty(
                "Reverse",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            this.matcher = matcher;
        }

        public override string CheckDescription => $"Not {this.matcher.CheckDescription}";

        public override bool Matches(object actual)
        {
            if (this.matcher.Matches(actual))
            {
                this.MatcherOutput.Append(this.matcher.MatcherOutput);
                return false;
            }
            
            return true;
        }
    }
}
