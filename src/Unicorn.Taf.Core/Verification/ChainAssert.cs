using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification
{
    /// <summary>
    /// Provides mechanism of soft chain assertions which allows 
    /// to perform multiple chacks and make final assertion after all checks.<para/>
    /// Final result combines all failed soft checks.
    /// </summary>
    public class ChainAssert
    {
        private const string But = "But: ";
        private const string Expected = "Expected: ";

        private readonly StringBuilder errors;
        private bool isSomethingFailed;
        private int errorCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainAssert"/> class 
        /// </summary>
        public ChainAssert()
        {
            this.errors = new StringBuilder();
            this.isSomethingFailed = false;
            this.errorCounter = 1;
        }

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is not specified by type and with specified message on fail
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="message">message thrown on fail</param>
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

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        public ChainAssert That(object actual, TypeUnsafeMatcher matcher) => That(actual, matcher, string.Empty);

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is suitable for specified actual object type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">message thrown on fail</param>
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

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        public ChainAssert That<T>(T actual, TypeSafeMatcher<T> matcher) => That<T>(actual, matcher, string.Empty);

        /// <summary>
        /// Perform soft check on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="message">message thrown on fail</param>
        public ChainAssert That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string message)
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

        /// <summary>
        /// Perform assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        public ChainAssert That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) => That<T>(actual, matcher, string.Empty);

        /// <summary>
        /// Perform final assertion of all checks in the chain
        /// </summary>
        public void AssertChain()
        {
            if (this.isSomethingFailed)
            {
                throw new AssertionException("Chain assertion failed with next errors" + this.errors.ToString().Trim());
            }
        }
    }
}
