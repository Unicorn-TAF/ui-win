using System.Reflection;

namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
{
    public class NotMatcher : Matcher
    {
        private Matcher matcher;

        public NotMatcher(Matcher matcher)
        {
            FieldInfo partOfNotMatcherField = typeof(Matcher).GetField(
                "reverse",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            this.matcher = matcher;
            ////this.matcher.MatcherOutput = this.MatcherOutput;
        }

        public override string CheckDescription => $"Not ({this.matcher.CheckDescription})";

        public override bool Matches(object obj)
        {
            bool result = IsNotNull(obj);

            if (result)
            {
                result = !this.matcher.Matches(obj);

                if (!result)
                {
                    this.matcher.DescribeMismatch(obj);
                }
            }

            this.MatcherOutput.Clear().Append(this.matcher.MatcherOutput);
            return result;
        }
    }
}
