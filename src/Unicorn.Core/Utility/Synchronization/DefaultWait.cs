using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Utility.Synchronization
{
    public abstract class DefaultWait<T>
    {
        protected static TimeSpan defaultSleepTimeout = TimeSpan.FromMilliseconds(250);
        protected static TimeSpan defaultTimeout = TimeSpan.FromSeconds(60);

        protected T input;
        protected SystemClock clock;
        protected List<Type> ignoredExceptions; 

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="input">The input value to pass to the evaluated conditions.</param>
        protected DefaultWait(T input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input", "input cannot be null");
            }

            this.input = input;
            this.clock = new SystemClock();
            this.ignoredExceptions = new List<Type>();
        }

        /// <summary>
        /// Gets or sets how long to wait for the evaluated condition to be true. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = defaultTimeout;

        /// <summary>
        /// Gets or sets how often the condition should be evaluated. The default timeout is 500 milliseconds.
        /// </summary>
        public TimeSpan PollingInterval { get; set; } = defaultSleepTimeout;

        /// <summary>
        /// Gets or sets the message to be displayed when time expires.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
        /// Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
        /// </summary>
        /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
        public void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            if (exceptionTypes == null)
            {
                throw new ArgumentNullException("exceptionTypes", "exceptionTypes cannot be null");
            }

            foreach (Type exceptionType in exceptionTypes)
            {
                if (!typeof(Exception).IsAssignableFrom(exceptionType))
                {
                    throw new ArgumentException("All types to be ignored must derive from System.Exception", "exceptionTypes");
                }
            }

            this.ignoredExceptions.AddRange(exceptionTypes);
        }

        protected bool IsIgnoredException(Exception exception)
        {
            return this.ignoredExceptions.Any(type => type.IsAssignableFrom(exception.GetType()));
        }
    }
}
