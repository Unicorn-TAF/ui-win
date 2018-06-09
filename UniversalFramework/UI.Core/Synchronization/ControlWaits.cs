using System;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.Synchronization
{
    public static class ControlWaits
    {
        private static readonly TimeSpan DefaultPollingInterval = TimeSpan.FromSeconds(0.5);
        private static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(30);

        private static TReturn WaitTill<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, Type ignoreException, string message = null) where TTarget : IControl
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

        private static TReturn WaitTill<TTarget, TReturn>(this TTarget control, Func<TTarget, TReturn> command, TimeSpan commandTimeout, TimeSpan pollingInterval, string message = null) where TTarget : IControl
        {
            var wait = new DefaultWait<TTarget>(control)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, commandTimeout),
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        private static TReturn WaitTillAttribute<TTarget, TReturn>(this TTarget control, Func<TTarget, string, string, TReturn> command, string attribute, string value, TimeSpan commandTimeout, TimeSpan pollingInterval, string message = null) where TTarget : IControl
        {
            var wait = new AttributeWait<TTarget>(control, attribute, value)
            {
                Message = message ?? string.Format("{0} expired after {1}", command, commandTimeout),
                PollingInterval = pollingInterval,
                Timeout = commandTimeout
            };

            return wait.Until(command);
        }

        public static TTarget WaitForAttributeContains<TTarget>(this TTarget control, string attribute, string value) where TTarget : class, IControl
        {
            return control.WaitTillAttribute(AttributeContains, attribute, value, DefaultCommandTimeout, DefaultPollingInterval, $"value '{value}' was not appeared in '{attribute}' attribute");
        }

        public static TTarget WaitForAttributeDoesNotContain<TTarget>(this TTarget control, string attribute, string value) where TTarget : class, IControl
        {
            return control.WaitTillAttribute(AttributeDoesNotContain, attribute, value, DefaultCommandTimeout, DefaultPollingInterval, $"value '{value}' was not disappeared from '{attribute}' attribute");
        }

        public static TTarget WaitForVisible<TTarget>(this TTarget control) where TTarget : class, IControl
        {
            return control.WaitTill(IsVisible, DefaultCommandTimeout, DefaultPollingInterval, $"{control} did not became visible");
        }

        public static TTarget WaitForEnabled<TTarget>(this TTarget control) where TTarget : class, IControl
        {
            return control.WaitTill(IsEnabled, DefaultCommandTimeout, DefaultPollingInterval, $"{control} did not became enabled");
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget AttributeContains<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl
        {
            return (element as IControl).GetAttribute(attribute).Contains(value) ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget AttributeDoesNotContain<TTarget>(this TTarget element, string attribute, string value) where TTarget : class, IControl
        {
            return !(element as IControl).GetAttribute(attribute).Contains(value) ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget IsVisible<TTarget>(this TTarget element) where TTarget : class, IControl
        {
            return element.Visible ? element : null;
        }

        /// <summary>
        ///     Checks weather element exist in DOM and visible.
        /// </summary>
        /// <typeparam name="TTarget">Target element type</typeparam>
        /// <param name="element">Element to check</param>
        /// <returns><c>true</c> when element exist in DOM and <c>false</c> otherwise</returns>
        private static TTarget IsEnabled<TTarget>(this TTarget element) where TTarget : class, IControl
        {
            return element.Enabled ? element : null;
        }
    }
}
