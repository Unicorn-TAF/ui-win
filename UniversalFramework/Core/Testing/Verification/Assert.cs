using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Core.Testing.Verification
{
    public class Assert
    {
        public static void That(object obj, Matcher matcher, string message = "")
        {
            matcher.MatcherOutput.Append("Expected: ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But: ");

            if (!matcher.Matches(obj))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                throw new AssertionError("\n" + message + matcher.MatcherOutput.ToString());
            }
        }
    }
}
