using System;
using System.Collections.Generic;
using System.Text;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Verification
{
    /// <summary>
    /// Provides mechanism of soft chain assertions which allows 
    /// to perform multiple checks and make final assertion after all checks.<para/>
    /// Final result combines all failed soft checks.
    /// </summary>
    public class ChainAssert
    {
        private const string But = "But: ";
        private const string Expected = "Expected: ";
        private const string FailedMessage = " failed with next errors";
        private const string DefaultDescription = "Chain assertion";

        private readonly string _errorMessage;
        private readonly StringBuilder _errors;
        private bool _isSomethingFailed;
        private int _errorCounter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainAssert"/> class. 
        /// </summary>
        public ChainAssert() : this(DefaultDescription)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChainAssert"/> class with specified description.
        /// </summary>
        /// <param name="description">check description</param>
        public ChainAssert(string description)
        {
            _errors = new StringBuilder();
            _isSomethingFailed = false;
            _errorCounter = 1;
            _errorMessage = description + FailedMessage;
        }

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is not specified by type and with specified message on fail
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="message">message thrown on fail</param>
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
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

                _errors.AppendLine($"Error {_errorCounter++}").Append(message).Append(matcher.Output.ToString()).AppendLine().AppendLine();
                _isSomethingFailed = true;
            }

            return this;
        }

        /// <summary>
        /// Perform soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
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
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
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

                _errors.AppendLine($"Error {_errorCounter++}").Append(message).Append(matcher.Output.ToString()).AppendLine().AppendLine();
                _isSomethingFailed = true;
            }

            return this;
        }

        /// <summary>
        /// Perform soft check on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
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
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
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

                _errors.AppendLine($"Error {_errorCounter++}").Append(message).Append(matcher.Output.ToString()).AppendLine().AppendLine();
                _isSomethingFailed = true;
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
        /// <returns>current <see cref="ChainAssert"/> instance</returns>
        public ChainAssert That<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) => That<T>(actual, matcher, string.Empty);

        /// <summary>
        /// Perform final assertion of all checks in the chain
        /// </summary>
        public void AssertChain()
        {
            if (_isSomethingFailed)
            {
                throw new AssertionException(_errorMessage + Environment.NewLine + _errors.ToString().Trim());
            }
        }
    }
}
