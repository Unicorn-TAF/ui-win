using Unicorn.Core.Testing.Assertions.Matchers;

namespace Unicorn.Core.Testing.Assertions
{
    public class Assertion
    {
        public static void AssertThat(object obj, Matcher matcher, string message = "")
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
