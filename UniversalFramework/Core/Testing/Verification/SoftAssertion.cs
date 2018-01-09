using System.Text;
using Unicorn.Core.Testing.Verification.Matchers;

namespace Unicorn.Core.Testing.Verification
{
    public class SoftAssertion
    {
        private StringBuilder errors;
        private bool isSomethingFailed;
        private int errorCounter;

        public SoftAssertion()
        {
            this.errors = new StringBuilder();
            this.isSomethingFailed = false;
            this.errorCounter = 1;
        }

        public SoftAssertion AssertThat(object obj, Matcher matcher)
        {
            this.AssertThat(string.Empty, obj, matcher);
            return this;
        }

        public SoftAssertion AssertThat(string message, object obj, Matcher matcher)
        {
            matcher.MatcherOutput.Append("Expected ");
            matcher.DescribeTo();
            matcher.MatcherOutput.AppendLine(string.Empty).Append("But ");

            if (!matcher.Matches(obj))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += "\n";
                }

                this.errors.AppendLine($"Error {errorCounter++}").Append(message).Append(matcher.ToString()).Append("\n\n");
                this.isSomethingFailed = true;
            }

            return this;
        }

        public void AssertAll()
        {
            if (this.isSomethingFailed)
            {
                throw new AssertionError("\n" + this.errors.ToString().Trim());
            }
        }
    }
}
