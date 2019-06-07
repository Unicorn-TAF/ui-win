using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification
{
    public static class Assert
    {
        private const string But = "But: ";
        private const string Expected = "Expected: ";

        public static void That(object actual, TypeUnsafeMatcher matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        public static void That(object actual, TypeUnsafeMatcher matcher) => That(actual, matcher, string.Empty);

        public static void That<T>(T actual, TypeSafeMatcher<T> matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        public static void That<T>(T actual, TypeSafeMatcher<T> matcher) => That(actual, matcher, string.Empty);

        public static void That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string message)
        {
            matcher.Output
                .Append(Expected)
                .Append(matcher.CheckDescription)
                .AppendLine()
                .Append(But);

            if (!matcher.Matches(actual))
            {
                var errorText = matcher.Output.ToString();

                if (!string.IsNullOrEmpty(message))
                {
                    errorText = message + Environment.NewLine + errorText;
                }

                throw new AssertionException(errorText);
            }
        }

        public static void That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) => That(actual, matcher, string.Empty);
    }
}
