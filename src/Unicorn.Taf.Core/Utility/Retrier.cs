using System;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Utility
{
    /// <summary>
    /// Utility for actions retry with ability to filter by exceptions and preconditions execution.
    /// </summary>
    public class Retrier
    {
        private const string LogPrefix = "Retrier: ";

        private readonly int _attempts;

        private Type[] exceptionsToCatch = null;
        private Action beforeRetryAction = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Retrier"/> class with 1 retry attempt.
        /// </summary>
        public Retrier() : this(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Retrier"/> class with specified retry attempts count.
        /// </summary>
        /// <param name="attempts">retry attempts count</param>
        public Retrier(int attempts)
        {
            _attempts = attempts;
        }

        /// <summary>
        /// Specifies exceptions to retry on.
        /// </summary>
        /// <param name="expectedExceptions">exceptions to catch</param>
        /// <returns><see cref="Retrier"/> instance</returns>
        public Retrier OnExceptions(params Type[] expectedExceptions)
        {
            exceptionsToCatch = expectedExceptions;
            return this;
        }

        /// <summary>
        /// Specifies action to execute before retry attempts.
        /// </summary>
        /// <param name="beforeAction">action to perform before each retry</param>
        /// <returns><see cref="Retrier"/> instance</returns>
        public Retrier DoBeforeRetry(Action beforeAction)
        {
            beforeRetryAction = beforeAction;
            return this;
        }

        /// <summary>
        /// Executes retry on specified action.
        /// </summary>
        /// <param name="action">action to retry</param>
        /// <exception cref="ArgumentNullException">is thrown if action to execute is null</exception>
        public void Execute(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            ExecuteRetry(action, 0);
        }

        /// <summary>
        /// Executes retry on specified action.
        /// </summary>
        /// <typeparam name="T">action return type</typeparam>
        /// <param name="func">action to retry</param>
        /// <returns>action result</returns>
        /// <exception cref="ArgumentNullException">is thrown if action to execute is null</exception>
        public T Execute<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            return ExecuteRetry(func, 0);
        }

        private void ExecuteRetry(Action action, int currentAttempt)
        {
            try
            {
                action.DynamicInvoke();
            }
            catch (TargetInvocationException ex)
            {
                Exception innerEx = ex.InnerException;

                if (currentAttempt++ < _attempts && (IsExpectedExceptionCaught(innerEx)))
                {
                    ULog.Warn("{0}found exception {1}: '{2}'. Retrying ({3})...",
                        LogPrefix, innerEx.GetType(), innerEx.Message, currentAttempt);

                    beforeRetryAction?.DynamicInvoke();
                    ExecuteRetry(action, currentAttempt);
                }
                else
                {
                    throw innerEx != null ? innerEx : ex;
                }
            }
        }

        private T ExecuteRetry<T>(Func<T> func, int currentAttempt)
        {
            try
            {
                return (T)func.DynamicInvoke();
            }
            catch (TargetInvocationException ex)
            {
                Exception innerEx = ex.InnerException;

                if (currentAttempt++ < _attempts && (IsExpectedExceptionCaught(innerEx)))
                {
                    ULog.Warn("{0}found exception {1}: '{2}'. Retrying ({3})...",
                        LogPrefix, innerEx.GetType(), innerEx.Message, currentAttempt);

                    beforeRetryAction?.DynamicInvoke();
                    return ExecuteRetry(func, currentAttempt);
                }
                else
                {
                    throw innerEx != null ? innerEx : ex;
                }
            }
        }

        private bool IsExpectedExceptionCaught(Exception ex)
        {
            var exType = ex.GetType();
            return exceptionsToCatch == null || exceptionsToCatch.Any(e => exType == e || exType.IsSubclassOf(e));
        }
    }
}
