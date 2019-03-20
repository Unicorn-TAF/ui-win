using System;
using System.Globalization;
using System.Threading;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Utility.Synchronization
{
    public class DefaultWait : AbstractWait
    {
        public void Until(Func<bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException("condition", "condition cannot be null");
            }

            Logger.Instance.Log(LogLevel.Debug, $"Waiting for '{condition.Method.Name} during {this.Timeout} with polling interval {this.PollingInterval}");

            Exception lastException = null;
            var endTime = this.Clock.LaterBy(this.Timeout);
            var startTime = DateTime.Now;

            while (true)
            {
                try
                {
                    if (condition.Invoke())
                    {
                        Logger.Instance.Log(LogLevel.Trace, $"wait is successful [Wait time = {DateTime.Now - startTime}]");
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

                // Check the timeout after evaluating the function to ensure conditions
                // with a zero timeout can succeed.
                if (!this.Clock.IsNowBefore(endTime))
                {
                    var timeoutMessage = string.IsNullOrEmpty(this.Message) ?
                        string.Format("{0} expired after {1} seconds", condition.Method.Name, Timeout.TotalSeconds) :
                        string.Format(CultureInfo.InvariantCulture, "Timed out after {0} seconds: {1}", this.Timeout.TotalSeconds, this.Message);

                    throw new TimeoutException(timeoutMessage, lastException);
                }

                Thread.Sleep(this.PollingInterval);
            }
        }
    }
}
