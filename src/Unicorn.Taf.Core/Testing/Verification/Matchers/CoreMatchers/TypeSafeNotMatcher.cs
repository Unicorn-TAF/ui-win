using System.Reflection;

namespace Unicorn.Core.Testing.Verification.Matchers.CoreMatchers
{
    public class TypeSafeNotMatcher<T> : TypeSafeMatcher<T>
    {
        private readonly TypeSafeMatcher<T> matcher;

        public TypeSafeNotMatcher(TypeSafeMatcher<T> matcher)
        {
            PropertyInfo partOfNotMatcherField = typeof(TypeSafeMatcher<T>).GetProperty(
                "Reverse",
                BindingFlags.NonPublic | BindingFlags.Instance);
            partOfNotMatcherField.SetValue(matcher, true);

            this.matcher = matcher;
        }

        public override string CheckDescription => $"Not {this.matcher.CheckDescription}";

        public override bool Matches(T actual)
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
