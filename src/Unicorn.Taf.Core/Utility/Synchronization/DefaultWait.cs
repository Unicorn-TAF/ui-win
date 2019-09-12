using System;
using System.Threading;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Utility.Synchronization
{
    /// <summary>
    /// Basic realization of simple wait for some boolean condition during specified timeout and with polling interval.
    /// </summary>
    public class DefaultWait : AbstractWait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait"/> class with default timeout and polling interval.
        /// </summary>
        public DefaultWait() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait"/> class with specified timeout and polling interval.
        /// </summary>
        /// <param name="timeout">wait timeout</param>
        /// <param name="pollingInterval">check polling interval</param>
        public DefaultWait(TimeSpan timeout, TimeSpan pollingInterval)
        {
            this.Timeout = timeout;
            this.PollingInterval = pollingInterval;
        }

        /// <summary>
        /// Wait until specified condition is met
        /// </summary>
        /// <param name="condition">boolean function specifying condition to wait for</param>
        /// <exception cref="TimeoutException">thrown when wait condition is not met</exception> 
        public void Until(Func<bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "Wait condition is not defined.");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for '{condition.Method.Name} during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            this.Timer
                .SetExpirationTimeout(this.Timeout)
                .Start();

            while (true)
            {
                try
                {
                    if (condition.Invoke())
                    {
                        Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {this.Timer.Elapsed}]");
                        return;
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
