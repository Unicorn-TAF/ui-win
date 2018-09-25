using System.Text;
using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Core.Testing.Verification
{
    public class Verify
    {
        private readonly StringBuilder errors;
        private bool isSomethingFailed;
        private int errorCounter;

        public Verify()
        {
            this.errors = new StringBuilder();
            this.isSomethingFailed = false;
            this.errorCounter = 1;
        }

        public Verify VerifyThat(object actual, Matcher matcher, string message = "")
        {
            matcher.MatcherOutput.Append("Expected ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But ");

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                this.errors.AppendLine($"Error {errorCounter++}").Append(message).Append(matcher.MatcherOutput.ToString()).Append("\n\n");
                this.isSomethingFailed = true;
            }

            return this;
        }

        public Verify VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher, string message = "")
        {
            matcher.MatcherOutput.Append("Expected ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But ");

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                this.errors.AppendLine($"Error {errorCounter++}").Append(message).Append(matcher.MatcherOutput.ToString()).Append("\n\n");
                this.isSomethingFailed = true;
            }

            return this;
        }

        public void AssertAll()
        {
            if (this.isSomethingFailed)
            {
                throw new AssertionException("\n" + this.errors.ToString().Trim());
            }
        }
    }
}
