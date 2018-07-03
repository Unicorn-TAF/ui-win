using System.Reflection;

namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
{
    public class NotMatcher : Matcher
    {
        private Matcher matcher;

        public NotMatcher(Matcher matcher)
        {
            PropertyInfo partOfNotMatcherField = typeof(Matcher).GetProperty(
                "Reverse",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            this.matcher = matcher;
        }

        public override string CheckDescription => $"Not ({this.matcher.CheckDescription})";

        public override bool Matches(object obj)
        {
            if (this.matcher.Matches(obj))
            {
                this.MatcherOutput.Clear().Append(this.matcher.MatcherOutput);
                return false;
            }
            
            return true;
        }
    }
}
