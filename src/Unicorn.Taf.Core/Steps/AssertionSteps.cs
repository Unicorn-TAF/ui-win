using System;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Verification;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.Taf.Core.Steps
{
    [Aspect(typeof(StepsEvents))]
    public class AssertionSteps
    {
        private ChainAssert chaninAssert = null;

        [Step("Assert that '{0}' {1}")]
        public void AssertThat<T>(T actual, TypeSafeMatcher<T> matcher) => 
            Assert.That(actual, matcher);

        [Step("Assert that '{0}' {1}")]
        public void AssertThat(object actual, TypeUnsafeMatcher matcher) =>
            Assert.That(actual, matcher);

        public AssertionSteps StartVerification()
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

        [Step("Assert verifications chain")]
        public void AssertVerificationsChain()
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
    }
}
