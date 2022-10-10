using System;
using System.Threading;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    /// <summary>
    /// UI controls specified wait (for <see cref="IControl"/>)
    /// </summary>
    /// <typeparam name="T">should implement <see cref="IControl"/></typeparam>
    public class UiWait<T> : BaseWait where T : IControl
    {
        private const string WaitSuccessTemplate = @"wait is successful [wait time = {0:mm\:ss\.fff}]";

        private readonly string _attributeName;
        private readonly string _valueValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        public UiWait(T input) : base()
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Input = input;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UiWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        /// <param name="attribute">UI control attribute name</param>
        /// <param name="value">UI control attribute value</param>
        public UiWait(T input, string attribute, string value) : base()
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            Input = input;

            _attributeName = attribute;
            _valueValue = value;
        }

        /// <summary>
        /// Input object to check wait condition against.
        /// </summary>
        protected T Input { get; set; }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException($"Can only wait on an object or boolean response, tried to use type: " + resultType, "condition");
            }

            ULog.Debug("Waiting for {0} {1} during {2:mm\\:ss\\.fff} with polling interval {3:mm\\:ss\\.fff}",
                Input, condition.Method.Name, Timeout, PollingInterval);

            Exception lastException = null;
            Timer
                .SetExpirationTimeout(Timeout)
                .Start();

            while (true)
            {
                try
                {
                    var result = condition(Input);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            ULog.Trace(WaitSuccessTemplate, Timer.Elapsed);
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            ULog.Trace(WaitSuccessTemplate, Timer.Elapsed);
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // throw TimeoutException if conditions are not met before timer expiration
                if (Timer.Expired)
                {
                    throw new TimeoutException(GenerateTimeoutMessage(condition.Method.Name), lastException);
                }

                Thread.Sleep(PollingInterval);
            }
        }

        /// <summary>
        /// Repeatedly applies this instance's input value to the given function until one of the following
        /// occurs:
        /// <para>
        /// <list type="bullet">
        /// <item>the function returns neither null nor false</item>
        /// <item>the function throws an exception that is not in the list of ignored exception types</item>
        /// <item>the timeout expires</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The delegate's expected return type.</typeparam>
        /// <param name="condition">A delegate taking an object of type T as its parameter, and returning a TResult.</param>
        /// <returns>The delegate's return value.</returns>
        public TResult UntilAttribute<TResult>(Func<T, string, string, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType, "condition");
            }

            ULog.Debug("Waiting for {0} '{1}' {2} '{3}' during {4:mm\\:ss\\.fff} with polling interval {5:mm\\:ss\\.fff}",
                Input, _attributeName, condition.Method.Name, _valueValue, Timeout, PollingInterval);

            Exception lastException = null;
            Timer
                .SetExpirationTimeout(Timeout)
                .Start();

            while (true)
            {
                try
                {
                    var result = condition(Input, _attributeName, _valueValue);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            ULog.Trace(WaitSuccessTemplate, Timer.Elapsed);
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            ULog.Trace(WaitSuccessTemplate, Timer.Elapsed);
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // throw TimeoutException if conditions are not met before timer expiration
                if (Timer.Expired)
                {
                    throw new TimeoutException(GenerateTimeoutMessage(condition.Method.Name), lastException);
                }

                Thread.Sleep(PollingInterval);
            }
        }
    }
}
