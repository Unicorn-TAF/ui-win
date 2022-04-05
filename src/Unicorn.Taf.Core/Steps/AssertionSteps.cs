using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// From the box implementation of steps for different kind of assertions:<para/>
    /// - typified/non-typified object checks<para/>
    /// - typified/non-typified objects collection checks<para/>
    /// - chain assertions on typified/non-typified objects<para/>
    /// - chain assertions on typified/non-typified objects collection<para/>
    /// </summary>
    public class AssertionSteps
    {
        private ChainAssert _chaninAssert = null;

        /// <summary>
        /// Step which performs assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="errorMessage">message thrown on fail</param>
        [Step("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher, string errorMessage) =>
            WrapStep(() => Assert.That(actual, matcher, errorMessage), actual, matcher, errorMessage);

        /// <summary>
        /// Step which performs assertion on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        [Step("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher) =>
            WrapStep(() => Assert.That(actual, matcher), actual, matcher);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// and with specified message on fail
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="errorMessage">message thrown on fail</param>
        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string errorMessage) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual, errorMessage);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual);

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type and with specified message on fail
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="errorMessage">message thrown on fail</param>
        [Step("Assert that '{0}' {1}")]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher, string errorMessage) =>
            WrapStep(() => Assert.That(actual, matcher, errorMessage), actual, matcher, errorMessage);

        /// <summary>
        /// Perform assertion on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        [Step("Assert that '{0}' {1}")]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher) =>
            WrapStep(() => Assert.That(actual, matcher), actual, matcher);

        /// <summary>
        /// Initializes assertions chain.
        /// </summary>
        /// <returns>current assertion steps instance</returns>
        /// <param name="description">description for following assertions</param>
        [Step("{0}")]
        public AssertionSteps StartAssertionsChain(string description) =>
            WrapStep(() =>
            {
                _chaninAssert = new ChainAssert(description);
                return this;
            }, description);

        /// <summary>
        /// Initializes assertions chain.
        /// </summary>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps StartAssertionsChain() => 
            StartAssertionsChain("Starting assertions chain");

        /// <summary>
        /// Step which performs soft check on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher) =>
            WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher);
                return this;
            }, actual, matcher);

        /// <summary>
        /// Step which performs soft check on object of any type using type specific matcher 
        /// which is suitable for specified actual object type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="errorMessage">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher, string errorMessage) =>
            WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher, errorMessage);
                return this;
            }, actual, matcher, errorMessage);

        /// <summary>
        /// Step which performs soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat(object actual, TypeUnsafeMatcher matcher) =>
            WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher);
                return this;
            }, actual, matcher);

        /// <summary>
        /// Step which performs soft check on object of any type using matcher 
        /// which is not specified by type
        /// </summary>
        /// <param name="actual">object to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeUnsafeMatcher"/> instance</param>
        /// <param name="errorMessage">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat(object actual, TypeUnsafeMatcher matcher, string errorMessage) =>
            WrapStep(() =>
            {
                if (_chaninAssert == null)
                {
                    _chaninAssert = new ChainAssert();
                }

                _chaninAssert.That(actual, matcher, errorMessage);
                return this;
            }, actual, matcher, errorMessage);

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps VerifyThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher)
        {
            if (_chaninAssert == null)
            {
                _chaninAssert = new ChainAssert();
            }

            ReportedCollectionVerifyThat(typeof(T).Name, matcher, actual);
            return this;
        }

        /// <summary>
        /// Step which performs assertion on collection of objects of same type using matcher 
        /// which is suitable for specified actual objects type
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="actual">collection of objects to perform assertion on</param>
        /// <param name="matcher"><see cref="TypeSafeMatcher{T}"/> instance</param>
        /// <param name="errorMessage">error message displayed on fail</param>
        /// <returns>current assertion steps instance</returns>
        public AssertionSteps VerifyThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string errorMessage)
        {
            if (_chaninAssert == null)
            {
                _chaninAssert = new ChainAssert();
            }

            ReportedCollectionVerifyThat(typeof(T).Name, matcher, actual, errorMessage);
            return this;
        }

        /// <summary>
        /// Step which performs assertion on chain of soft asserts performed after chain initialization.
        /// </summary>
        /// <exception cref="InvalidOperationException">is thrown if chain assert was not initialized</exception>
        [Step("Assert verifications chain")]
        public void AssertChain() =>
            WrapStep(() => 
            {
                if (_chaninAssert == null)
                {
                    throw new InvalidOperationException(
                        "There were no any verifications made. Please check scenario.");
                }

                try
                {
                    _chaninAssert.AssertChain();
                }
                finally
                {
                    _chaninAssert = null;
                }
            });

        [Step("Assert that collection of <{0}> {1}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", 
            Justification = "'elementType' is used just for automatic reporting ")]
        private void ReportedCollectionAssertThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual, string errorMessage) =>
            WrapStep(() => Assert.That(actual, matcher, errorMessage), elementType, matcher, actual, errorMessage);

        [Step("Assert that collection of <{0}> {1}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", 
            Justification = "'elementType' is used just for automatic reporting ")]
        private void ReportedCollectionAssertThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            WrapStep(() => Assert.That(actual, matcher), elementType, matcher, actual);

        [Step("Assert that collection of <{0}> {1}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", 
            Justification = "'elementType' is used just for automatic reporting ")]
        private void ReportedCollectionVerifyThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            WrapStep(() => _chaninAssert.That(actual, matcher), elementType, matcher, actual);

        [Step("Assert that collection of <{0}> {1}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", 
            Justification = "'elementType' is used just for automatic reporting ")]
        private void ReportedCollectionVerifyThat<T>(
            string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual, string errorMessage) =>
            WrapStep(() => _chaninAssert.That(actual, matcher, errorMessage), elementType, matcher, actual, errorMessage);

        private void WrapStep(Action action, params object[] arguments)
        {
            MethodBase methodBase = new StackFrame(1).GetMethod();

            try
            {
                StepEvents.CallOnStepStartEvent(methodBase, arguments);
                action();
            }
            finally
            {
                StepEvents.CallOnStepFinishEvent(methodBase, arguments);
            }
        }

        private T WrapStep<T>(Func<T> action, params object[] arguments)
        {
            MethodBase methodBase = new StackFrame(1).GetMethod();

            try
            {
                StepEvents.CallOnStepStartEvent(methodBase, arguments);
                return action();
            }
            finally
            {
                StepEvents.CallOnStepFinishEvent(methodBase, arguments);
            }
        }
    }
}
