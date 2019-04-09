using System;
using System.Text;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Taf.Core.Verification
{
    public class ChainAssert
    {
        private const string But = "But: ";
        private const string Expected = "Expected: ";

        private readonly StringBuilder errors;
        private bool isSomethingFailed;
        private int errorCounter;

        public ChainAssert()
        {
            this.errors = new StringBuilder();
            this.isSomethingFailed = false;
            this.errorCounter = 1;
        }

        public ChainAssert That(object actual, TypeUnsafeMatcher matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                this.errors.AppendLine($"Error {errorCounter++}").Append(message).Append(matcher.Output.ToString()).AppendLine().AppendLine();
                this.isSomethingFailed = true;
            }

            return this;
        }

        public ChainAssert That(object actual, TypeUnsafeMatcher matcher) => That(actual, matcher, string.Empty);

        public ChainAssert That<T>(T actual, TypeSafeMatcher<T> matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                if (!string.IsNullOrEmpty(message))
                {
                    message += Environment.NewLine;
                }

                this.errors.AppendLine($"Error {errorCounter++}").Append(message).Append(matcher.Output.ToString()).AppendLine().AppendLine();
                this.isSomethingFailed = true;
            }

            return this;
        }

        public ChainAssert That<T>(T actual, TypeSafeMatcher<T> matcher) => That<T>(actual, matcher, string.Empty);

        public void AssertChain()
        {
            if (this.isSomethingFailed)
            {
                throw new AssertionException("Chain assertion failed with next errors" + this.errors.ToString().Trim());
            }
        }
    }
}
