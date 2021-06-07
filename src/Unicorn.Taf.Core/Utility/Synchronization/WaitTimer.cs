using System;

namespace Unicorn.Taf.Core.Utility.Synchronization
{
    /// <summary>
    /// Uses the system clock to calculate time for timeouts.
    /// </summary>
    public class WaitTimer
    {
        private DateTime expirationDateTime;

        /// <summary>
        /// Gets a value indicating if timer was expired or not
        /// </summary>
        public bool Expired => DateTime.Now > expirationDateTime;

        /// <summary>
        /// Gets or sets time when timer was started.
        /// </summary>
        public DateTime StartTime { get; protected set; }

        /// <summary>
        /// Gets a value indicating elapsed time
        /// </summary>
        public TimeSpan Elapsed => DateTime.Now - StartTime;

        /// <summary>
        /// Set the date and time of timer expiration.
        /// </summary>
        /// <param name="delay">expiration timeout</param>
        /// <returns>current <see cref="WaitTimer"/> instance</returns>
        public WaitTimer SetExpirationTimeout(TimeSpan delay)
        {
            expirationDateTime = DateTime.Now.Add(delay);
            return this;
        }
            
        /// <summary>
        /// Starts wait timer
        /// </summary>
        /// <returns>current <see cref="WaitTimer"/> instance</returns>
        public WaitTimer Start()
        {
            StartTime = DateTime.Now;
            return this;
        }
    }
}
