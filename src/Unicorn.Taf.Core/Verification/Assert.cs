using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Taf.Core.Verification
{
    public static class Assert
    {
        public static void That(object actual, Matcher matcher, string message = "")
        {
            matcher.MatcherOutput.Append("Expected: ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But: ");

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                throw new AssertionException("\n" + message + matcher.MatcherOutput);
            }
        }

        public static void That<T>(T actual, TypeSafeMatcher<T> matcher, string message = "")
        {
            matcher.MatcherOutput.Append("Expected: ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But: ");

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                throw new AssertionException("\n" + message + matcher.MatcherOutput);
            }
        }
    }
}
