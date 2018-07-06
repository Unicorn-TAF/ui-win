using System;
using System.Diagnostics;
using Unicorn.Core.Logging;

namespace Unicorn.Core.Utility
{
    public class Countdown
    {
        private readonly Stopwatch timer;
        private TimeSpan timeLimit;

        public Countdown(TimeSpan timeLimit)
        {
            this.timer = new Stopwatch();
            this.timeLimit = timeLimit;
        }

        public TimeSpan Elapsed => this.timer.Elapsed;

        public bool Expired => this.timer.Elapsed >= this.timeLimit;

        public static Countdown StartNew(TimeSpan timeLimit)
        {
            var countdown = new Countdown(timeLimit);
            countdown.Start();
            return countdown;
        }

        public void SetTimeLimit(TimeSpan timeLimitValue)
        {
            this.timeLimit = timeLimitValue;
        }

        public void Start()
        {
            this.timer.Start();
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        public void Restart()
        {
            this.timer.Restart();
        }

        public void Reset()
        {
            this.timer.Reset();
        }

        public void ThrowExceptionIfExpired(string message)
        {
            if (Expired)
            {
                this.timer.Stop();
                throw new TimeoutException(message);
            }
        }

        public void LogMessageIfExpired(string message)
        {
            if (Expired)
            {
                this.timer.Stop();
                Logger.Instance.Log(LogLevel.Debug, message);
            }
        }
    }
}
