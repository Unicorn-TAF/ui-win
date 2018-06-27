using System;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public static class ControlWaits
    {
        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(30);

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, Type ignoreException, string message = null) where TTarget : IControl
        {
            var wait = new DefaultWait<TTarget>(control)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, commandTimeout),
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            wait.IgnoreExceptionTypes(ignoreException);

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, string message = null) where TTarget : IControl
        {
            var wait = new DefaultWait<TTarget>(control)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, commandTimeout),
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, string message = null) where TTarget : IControl
        {
            var wait = new DefaultWait<TTarget>(control)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, DefaultCommandTimeout),
                PollingInterval = DefaultPollingInterval,
                Timeout = DefaultCommandTimeout
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, string, string, TReturn> command, string attribute, string value, string message = null) where TTarget : IControl
        {
            var wait = new AttributeWait<TTarget>(control, attribute, value)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, DefaultCommandTimeout),
                PollingInterval = DefaultPollingInterval,
                Timeout = DefaultCommandTimeout
            };

            return wait.Until(command);
        }
    }
}
