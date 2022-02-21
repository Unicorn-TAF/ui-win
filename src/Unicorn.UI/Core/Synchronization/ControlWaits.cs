using System;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    /// <summary>
    /// <see cref="IControl"/> specified waiters.
    /// </summary>
    public static class ControlWaits
    {
        #region Control state waits

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance ingoring specified exception.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="ignoreException">exception type to ignore</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance ingoring specified exception.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="ignoreException">exception type to ignore</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval, 
            Type ignoreException) where TTarget : IControl =>
            Wait(control, command, commandTimeout, pollingInterval, ignoreException, string.Empty);

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command, 
            TimeSpan commandTimeout, 
            TimeSpan pollingInterval) where TTarget : IControl =>
            Wait(control, command, commandTimeout, pollingInterval, string.Empty);

        /// <summary>
        /// During specified timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, TReturn> command,
            TimeSpan commandTimeout) where TTarget : IControl =>
            Wait(control, command, commandTimeout, string.Empty);

        /// <summary>
        /// During default timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During default timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> instance.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control, 
            Func<TTarget, TReturn> command) where TTarget : IControl =>
            Wait(control, command, string.Empty);

        #endregion

        #region Control attribute waits

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value ingoring specified exception.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="ignoreException">exception type to ignore</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value ingoring specified exception.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="ignoreException">exception type to ignore</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval,
            Type ignoreException) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, pollingInterval, ignoreException, string.Empty);

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with specified interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="pollingInterval">wait polling interval</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout,
            TimeSpan pollingInterval) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, pollingInterval, string.Empty);

        /// <summary>
        /// During specified timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During specified timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="commandTimeout">wait timeout</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value,
            TimeSpan commandTimeout) where TTarget : IControl =>
            Wait(control, command, attribute, value, commandTimeout, string.Empty);

        /// <summary>
        /// During default timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <param name="message">error message in case of fail</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
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

        /// <summary>
        /// During default timeout and with default interval waits for some condition to be met 
        /// for <see cref="IControl"/> specified attribute value.
        /// </summary>
        /// <typeparam name="TTarget">type of control under wait</typeparam>
        /// <typeparam name="TReturn">type of control under wait</typeparam>
        /// <param name="control">control under wait</param>
        /// <param name="command">wait condition</param>
        /// <param name="attribute">attribute to check value of</param>
        /// <param name="value">expected attribute value</param>
        /// <returns>control instance</returns>
        /// <exception cref="TimeoutException">is thrown if wait reached timeout</exception>
        public static TReturn Wait<TTarget, TReturn>(
            this TTarget control,
            Func<TTarget, string, string, TReturn> command,
            string attribute,
            string value) where TTarget : IControl =>
            Wait(control, command, attribute, value, string.Empty);

        #endregion
    }
}
