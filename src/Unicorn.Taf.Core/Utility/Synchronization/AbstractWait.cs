using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Taf.Core.Utility.Synchronization
{
    public abstract class AbstractWait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractWait"/> class.
        /// </summary>
        protected AbstractWait()
        {
            this.Timer = new WaitTimer();
            this.IgnoredExceptions = new List<Type>();
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

            this.IgnoredExceptions.AddRange(exceptionTypes);
        }

        /// <summary>
        /// Check if current exception type is in the list of ignored exceptions types
        /// </summary>
        /// <param name="exception">exception instance</param>
        /// <returns><see langword="true"/> if the current exception should be ignored; otherwise, <see langword="false"/>.</returns>
        protected bool IsIgnoredException(Exception exception) =>
            this.IgnoredExceptions.Any(type => type.IsAssignableFrom(exception.GetType()));

        protected string GenerateTimeoutMessage(string conditionName) =>
            string.IsNullOrEmpty(this.ErrorMessage) ? string.Empty : $"{this.ErrorMessage}: " + 
            string.Format("{0} expired after {1:F1} seconds", conditionName, Timeout.TotalSeconds);
    }
}
