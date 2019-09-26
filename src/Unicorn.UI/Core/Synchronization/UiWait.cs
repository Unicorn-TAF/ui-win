using System;
using System.Threading;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public class UiWait<T> : AbstractWait where T : IControl
    {
        private readonly string attributeName;
        private readonly string valueValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="UiWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        public UiWait(T input) : base()
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "input cannot be null");
            }

            this.Input = input;
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
                throw new ArgumentNullException("input", "input cannot be null");
            }

            this.Input = input;

            this.attributeName = attribute;
            this.valueValue = value;
        }

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
        public TResult Until<TResult>(Func<T, TResult> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException($"Can only wait on an object or boolean response, tried to use type: " + resultType, "condition");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for {Input} {condition.Method.Name} during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            this.Timer
                .SetExpirationTimeout(this.Timeout)
                .Start();

            while (true)
            {
                try
                {
                    var result = condition(this.Input);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {this.Timer.Elapsed}]");
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {this.Timer.Elapsed}]");
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!this.IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // throw TimeoutException if conditions are not met before timer expiration
                if (this.Timer.Expired)
                {
                    throw new TimeoutException(this.GenerateTimeoutMessage(condition.Method.Name), lastException);
                }

                Thread.Sleep(this.PollingInterval);
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
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            var resultType = typeof(TResult);
            if ((resultType.IsValueType && resultType != typeof(bool)) || !typeof(object).IsAssignableFrom(resultType))
            {
                throw new ArgumentException("Can only wait on an object or boolean response, tried to use type: " + resultType, "condition");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for {Input} '{this.attributeName}' {condition.Method.Name} '{this.valueValue}' during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            this.Timer
                .SetExpirationTimeout(this.Timeout)
                .Start();

            while (true)
            {
                try
                {
                    var result = condition(this.Input, this.attributeName, this.valueValue);
                    if (resultType == typeof(bool))
                    {
                        var boolResult = result as bool?;
                        if (boolResult.HasValue && boolResult.Value)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {this.Timer.Elapsed}]");
                            return result;
                        }
                    }
                    else
                    {
                        if (result != null)
                        {
                            Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {this.Timer.Elapsed}]");
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!this.IsIgnoredException(ex))
                    {
                        throw;
                    }

                    lastException = ex;
                }

                // throw TimeoutException if conditions are not met before timer expiration
                if (this.Timer.Expired)
                {
                    throw new TimeoutException(this.GenerateTimeoutMessage(condition.Method.Name), lastException);
                }

                Thread.Sleep(this.PollingInterval);
            }
        }
    }
}
