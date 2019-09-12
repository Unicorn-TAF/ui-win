using System;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public static class ControlWaits
    {
        #region Control state waits

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval, 
            Type ignoreException, 
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                ErrorMessage = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout,
            };

            wait.IgnoreExceptionTypes(ignoreException);

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval, 
            Type ignoreException) where TTarget : IControl =>
            Wait(control, command, commandTimeout, pollingInterval, ignoreException, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval, 
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                ErrorMessage = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval) where TTarget : IControl =>
            Wait(control, command, commandTimeout, pollingInterval, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, TReturn> command,
            TimeSpan commandTimeout,
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                ErrorMessage = message,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, TReturn> command,
            TimeSpan commandTimeout) where TTarget : IControl =>
            Wait(control, command, commandTimeout, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control)
            {
                ErrorMessage = message,
            };

            return wait.Until(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command) where TTarget : IControl =>
            Wait(control, command, string.Empty);

        #endregion

        #region Control attribute waits

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval,
            Type ignoreException,
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control, attribute, value)
            {
                ErrorMessage = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout,
            };

            wait.IgnoreExceptionTypes(ignoreException);

            return wait.UntilAttribute(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval,
            Type ignoreException) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, pollingInterval, ignoreException, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval,
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control, attribute, value)
            {
                ErrorMessage = message,
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.UntilAttribute(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, pollingInterval, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control, attribute, value)
            {
                ErrorMessage = message,
                Timeout = commandTimeout
            };

            return wait.UntilAttribute(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, string.Empty);

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, string, string, TReturn> command, 
            string attribute, 
            string value, 
            string message) where TTarget : IControl
        {
            var wait = new UiWait<TTarget>(control, attribute, value)
            {
                ErrorMessage = message,
            };

            return wait.UntilAttribute(command);
        }

        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value) where TTarget : IControl =>
            Wait(control, command, attribute, value, string.Empty);

        #endregion
    }
}
