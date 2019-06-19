using System;
using System.Collections.Generic;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.Taf.Core.Verification.Matchers.CollectionMatchers;

namespace Unicorn.Taf.Core.Steps
{
    [Inject(typeof(StepsEvents))]
    public class AssertionSteps
    {
        private ChainAssert chaninAssert = null;

        [Step("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher, string errorMessage) => 
            Assert.That(actual, matcher, errorMessage);

        [Step("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher) =>
            Assert.That(actual, matcher);

        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher, string errorMessage) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual, errorMessage);

        public void AssertThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher) =>
            ReportedCollectionAssertThat(typeof(T).Name, matcher, actual);

        [Step("Assert that '{0}' {1}")]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher, string errorMessage) =>
            Assert.That(actual, matcher, errorMessage);

        [Step("Assert that '{0}' {1}")]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher) =>
            Assert.That(actual, matcher);

        public AssertionSteps StartAssertionsChain()
        {
            chaninAssert = new ChainAssert();
            return this;
        }

        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat<T>(T actual, TypeSafeMatcher<T> matcher)
        {
            if (chaninAssert == null)
            {
                chaninAssert = new ChainAssert();
            }

            chaninAssert.That(actual, matcher);
            return this;
        }

        [Step("Verify that '{0}' {1}")]
        public AssertionSteps VerifyThat(object actual, TypeUnsafeMatcher matcher)
        {
            if (chaninAssert == null)
            {
                chaninAssert = new ChainAssert();
            }

            chaninAssert.That(actual, matcher);
            return this;
        }

        public AssertionSteps VerifyThat<T>(IEnumerable<T> actual, TypeSafeCollectionMatcher<T> matcher)
        {
            if (chaninAssert == null)
            {
                chaninAssert = new ChainAssert();
            }

            ReportedCollectionVerifyThat(typeof(T).Name, matcher, actual);
            return this;
        }

        [Step("Assert verifications chain")]
        public void AssertChain()
        {
            if (chaninAssert == null)
            {
                throw new InvalidOperationException("There were no any verifications made. Please check scenario.");
            }

            try
            {
                chaninAssert.AssertChain();
            }
            finally
            {
                chaninAssert = null;
            }
        }

        [Step("Assert that collection of <{0}> {1}")]
        private void ReportedCollectionAssertThat<T>(string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual, string errorMessage) =>
            Assert.That(actual, matcher, errorMessage);

        [Step("Assert that collection of <{0}> {1}")]
        private void ReportedCollectionAssertThat<T>(string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            Assert.That(actual, matcher);

        [Step("Assert that collection of <{0}> {1}")]
        private void ReportedCollectionVerifyThat<T>(string elementType, TypeSafeCollectionMatcher<T> matcher, IEnumerable<T> actual) =>
            chaninAssert.That(actual, matcher);
    }
}
