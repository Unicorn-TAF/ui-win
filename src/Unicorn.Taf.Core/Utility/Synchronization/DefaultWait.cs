using System;
using System.Threading;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Utility.Synchronization
{
    /// <summary>
    /// Basic implementation of simple wait for some boolean condition during 
    /// specified timeout and with polling interval.
    /// </summary>
    public class DefaultWait : BaseWait
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait"/> class with 
        /// 60 sec timeout and 250 ms polling interval.
        /// </summary>
        public DefaultWait() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait"/> class with 
        /// specified timeout and polling interval.
        /// </summary>
        /// <param name="timeout">wait timeout</param>
        /// <param name="pollingInterval">check polling interval</param>
        public DefaultWait(TimeSpan timeout, TimeSpan pollingInterval) : base()
        {
            Timeout = timeout;
            PollingInterval = pollingInterval;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultWait"/> class with 
        /// specified timeout and 250 ms polling interval.
        /// </summary>
        /// <param name="timeout">wait timeout</param>
        public DefaultWait(TimeSpan timeout) : base()
        {
            Timeout = timeout;
        }

        /// <summary>
        /// Waits until specified condition is met.
        /// </summary>
        /// <param name="condition">boolean function specifying condition to wait for</param>
        /// <exception cref="TimeoutException">thrown when wait condition is not met</exception> 
        public void Until(Func<bool> condition) => PerformWait(condition, true);

        /// <summary>
        /// Safely waits until specified condition is met and returns wait result as boolean.
        /// </summary>
        /// <param name="condition">boolean function specifying condition to wait for</param>
        /// <returns>true - if wait for condition is successful; otherwise - false</returns>
        public bool SafelyUntil(Func<bool> condition) => PerformWait(condition, false);

        private bool PerformWait(Func<bool> condition, bool failOnTimeout)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition), "Wait condition is not defined.");
            }

            ULog.Debug("Waiting for '{0} during {1:mm\\:ss\\.fff} with polling interval {2:mm\\:ss\\.fff}",
                condition.Method.Name, Timeout, PollingInterval);

            Exception lastException = null;
            Timer
                .SetExpirationTimeout(Timeout)
                .Start();

            while (true)
            {
                try
                {
                    if (condition.Invoke())
                    {
                        ULog.Trace(@"wait is successful [wait time = {0:mm\:ss\.fff}]", Timer.Elapsed);
                        return true;
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
                    var message = GenerateTimeoutMessage(condition.Method.Name);

                    if (failOnTimeout)
                    {
                        throw new TimeoutException(message, lastException);
                    }
                    else
                    {
                        ULog.Warn(message);
                        return false;
                    }
                }

                Thread.Sleep(PollingInterval);
            }
        }
    }
}
