using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Utility.Synchronization
{
    /// <summary>
    /// Provides base for implementation of waiters
    /// </summary>
    public abstract class BaseWait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWait"/> class.
        /// </summary>
        protected BaseWait()
        {
            Timer = new WaitTimer();
            IgnoredExceptions = new List<Type>();
        }

        /// <summary>
        /// Gets or sets how long to wait for the evaluated condition to be true.The default timeout is 60 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// Gets or sets how often the condition should be evaluated. The default timeout is 250 milliseconds.
        /// </summary>
        public TimeSpan PollingInterval { get; set; } = TimeSpan.FromMilliseconds(250);

        /// <summary>
        /// Gets or sets the message to be displayed when time expires.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets timer.
        /// </summary>
        protected WaitTimer Timer { get; set; }

        /// <summary>
        /// Gets list of Exceptions types to ignore while waiting
        /// </summary>
        protected List<Type> IgnoredExceptions { get; }

        /// <summary>
        /// Configures this instance to ignore specific types of exceptions while waiting for a condition.
        /// Any exceptions not whitelisted will be allowed to propagate, terminating the wait.
        /// </summary>
        /// <param name="exceptionTypes">The types of exceptions to ignore.</param>
        public void IgnoreExceptionTypes(params Type[] exceptionTypes)
        {
            foreach (Type exceptionType in exceptionTypes)
            {
                if (!typeof(Exception).IsAssignableFrom(exceptionType))
                {
                    throw new ArgumentException("All types to be ignored must derive from System.Exception", "exceptionTypes");
                }
            }

            IgnoredExceptions.AddRange(exceptionTypes);
        }

        /// <summary>
        /// Check if current exception type is in the list of ignored exceptions types
        /// </summary>
        /// <param name="exception">exception instance</param>
        /// <returns><see langword="true"/> if the current exception should be ignored; otherwise, <see langword="false"/>.</returns>
        protected bool IsIgnoredException(Exception exception) =>
            IgnoredExceptions.Any(type => type.IsAssignableFrom(exception.GetType()));

        /// <summary>
        /// Construct message to use in <see cref="TimeoutException"/> of waiter.
        /// </summary>
        /// <param name="conditionName">name of wait condition to report</param>
        /// <returns>message string</returns>
        protected string GenerateTimeoutMessage(string conditionName) =>
            string.IsNullOrEmpty(ErrorMessage) ? string.Empty : $"{ErrorMessage}: " + 
            string.Format("{0} expired after {1:F1} seconds", conditionName, Timeout.TotalSeconds);
    }
}
