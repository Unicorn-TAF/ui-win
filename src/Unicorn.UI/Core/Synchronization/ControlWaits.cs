using System;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public static class ControlWaits
    {
        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, Type ignoreException, string message = null) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                Message = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout,
            };

            wait.IgnoreExceptionTypes(ignoreException);

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, string message = null) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                Message = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, string message = null) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                Message = message,
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(this TTarget control, Func<TTarget, string, string, TReturn> command, string attribute, string value, string message = null) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control, attribute, value)
            {
                Message = message,
            };

            return wait.UntilAttribute(command);
        }
    }
}
