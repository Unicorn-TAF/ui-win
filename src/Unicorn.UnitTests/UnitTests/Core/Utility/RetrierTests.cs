using NUnit.Framework;
using System;
using Unicorn.Taf.Core.Utility;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Utility
{
    [TestFixture]
    public class RetrierTests : NUnitTestRunner
    {
        [Test(Description = "Catch specified exception specified number of times")]
        public void TestCatchSpecifiedExceptionSpecifiedNumberOfTimes()
        {
            int attempts = 0;

            try
            {
                new Retrier(2)
                    .OnExceptions(typeof(ArgumentException))
                    .Execute(() => 
                    { 
                        attempts++; 
                        throw new ArgumentException("expected"); 
                    });
            }
            catch (ArgumentException)
            {
                Assert.That(attempts, Is.EqualTo(3), "Incorrect attempts count executed");
            }
        }

        [Test(Description = "Catch any exception specified number of times")]
        public void TestCatchAnyExceptionSpecifiedNumberOfTimes()
        {
            int attempts = 0;

            try
            {
                new Retrier(2)
                    .Execute(() => 
                    { 
                        attempts++; 
                        throw new AggregateException("expected"); 
                    });
            }
            catch (AggregateException)
            {
                Assert.That(attempts, Is.EqualTo(3), "Incorrect attempts count executed");
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Retry pass if no exception")]
        public void TestRetryPassInNoException()
        {
            int attempts = 0;

            new Retrier(6)
                .OnExceptions(typeof(ArgumentException))
                .Execute(() => { attempts++; });

            Assert.That(attempts, Is.EqualTo(1), "Incorrect attempts count executed");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Retry returns execution result")]
        public void TestRetryReturnsExecutionResult()
        {
            string resultString = "result";

            var result = new Retrier()
                .OnExceptions(typeof(ArgumentException))
                .Execute(() => GetString());

            Assert.That(result, Is.EqualTo(resultString), "retry did not return execution result");

            string GetString() => resultString;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Catch several specified exceptions")]
        public void TestCatchSeveralSpecifiedExceptions()
        {
            int attempts = 0;

            try
            {
                new Retrier(5)
                    .OnExceptions(typeof(ArgumentException), typeof(ArithmeticException))
                    .Execute(() =>
                    {
                        attempts++;
                        switch (attempts)
                        {
                            case 1:
                                throw new ArgumentException("expected");
                            case 2:
                                throw new ArithmeticException("expected");
                            default:
                                throw new NullReferenceException("expected");
                        }
                    });
            }
            catch (NullReferenceException)
            {
                Assert.That(attempts, Is.EqualTo(3), "Incorrect attempts count executed");
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Ignore not specifiedException specified exception")]
        public void TestIgnoreNotSpecifiedException()
        {
            int attempts = 0;

            try
            {
                new Retrier(4)
                    .OnExceptions(typeof(NullReferenceException))
                    .Execute(() => 
                    { 
                        attempts++; 
                        throw new ArgumentException("expected"); 
                    });
            }
            catch (ArgumentException)
            {
                Assert.That(attempts, Is.EqualTo(1), "Incorrect attempts count executed");
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Before action is executed on retry")]
        public void TestBeforeActionIsExecutedOnRetry()
        {
            int attempts = 0;

            try
            {
                new Retrier(2)
                    .OnExceptions(typeof(ArgumentException))
                    .DoBeforeRetry(() => attempts++)
                    .Execute(() => { throw new ArgumentException("expected"); });
            }
            catch (ArgumentException)
            {
                Assert.That(attempts, Is.EqualTo(2), "Incorrect count of before actions executed");
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Before action is not executed if no retry")]
        public void TestBeforeActionIsNotExecutedIfNoRetry()
        {
            int attempts = 0;

            new Retrier(3)
                .OnExceptions(typeof(ArgumentException))
                .DoBeforeRetry(() => attempts++)
                .Execute(() => { });

            Assert.That(attempts, Is.EqualTo(0), "Incorrect count of before actions executed");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Before action fails if action is null")]
        public void TestRetryFailsIfActionIsNull()
        {
            try
            {
                new Retrier()
                    .Execute(null);

                throw new AssertionException("Retry should fail on null action");
            }
            catch (ArgumentException)
            {
                // positive case
            }
        }
    }
}
