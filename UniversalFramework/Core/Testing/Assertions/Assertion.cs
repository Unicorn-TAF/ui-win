using System;
using System.Text;
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

                throw new AssertionError(message + matcher.MatcherOutput.ToString());
            }
        }
    }

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

    [Serializable]
    public class AssertionError : Exception
    {
        public AssertionError() : base()
        {
        }

        public AssertionError(string message) : base(message)
        {
        }
    }
}
