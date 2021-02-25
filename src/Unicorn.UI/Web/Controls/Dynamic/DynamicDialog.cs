using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Web.PageObject;

namespace Unicorn.UI.Web.Controls.Dynamic
{
    /// <summary>
    /// Describes dynamically defined Dialog window (each sub-control could be defined using attribute).
    /// </summary>
    public class DynamicDialog : WebContainer, IDynamicDialog
    {
        /// <summary>
        /// Gets dialog title text.
        /// </summary>
        public virtual string Title => GetTitleControl().Text.Trim();

        /// <summary>
        /// Gets dialog text content.
        /// </summary>
        public virtual string TextContent => GetContentControl().GetAttribute("innerText").Trim();

        /// <summary>
        /// Gets dictionary of sub-elements locators.
        /// </summary>
        protected Dictionary<DialogElement, ByLocator> Locators = new Dictionary<DialogElement, ByLocator>();

        /// <summary>
        /// Populates sub-elements locators from input dictionary.
        /// </summary>
        /// <param name="elementsLocators">sub-elements locators dictionary</param>
        public void Populate(Dictionary<int, ByLocator> elementsLocators)
        {
            foreach (var locator in elementsLocators)
            {
                var key = (DialogElement)locator.Key;

                if (Locators.ContainsKey(key))
                {
                    Locators[key] = locator.Value;
                }
                else
                {
                    Locators.Add((DialogElement)locator.Key, locator.Value);
                }
            }
        }

        /// <summary>
        /// Gets control for dialog title.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetTitleControl() =>
            Locators.ContainsKey(DialogElement.Title) ?
            Find<WebControl>(Locators[DialogElement.Title]) :
            throw new NotSpecifiedLocatorException("Title dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog content.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetContentControl() =>
            Locators.ContainsKey(DialogElement.Content) ?
            Find<WebControl>(Locators[DialogElement.Content]) :
            throw new NotSpecifiedLocatorException("Content dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog acceptance button.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetAcceptButton() =>
            Locators.ContainsKey(DialogElement.Accept) ?
            Find<WebControl>(Locators[DialogElement.Accept]) :
            throw new NotSpecifiedLocatorException("Accept button dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog declining button.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetDeclineButton() =>
            Locators.ContainsKey(DialogElement.Decline) ?
            Find<WebControl>(Locators[DialogElement.Decline]) :
            throw new NotSpecifiedLocatorException("Decline button dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog close button.
        /// <exception cref="NotSpecifiedLocatorException">is thrown when sub-control was not defined</exception>
        /// </summary>
        /// <returns><see cref="IControl"/> instance</returns>
        public virtual IControl GetCloseButton() =>
            Locators.ContainsKey(DialogElement.Close) ?
            Find<WebControl>(Locators[DialogElement.Close]) :
            throw new NotSpecifiedLocatorException("Close button dialog sub-control locator is not specified.");

        /// <summary>
        /// Accepts a dialog and waits for window disappearance.
        /// </summary>
        public void Accept()
        {
            GetAcceptButton().Click();
            WaitForWindowIsNotDisplayed();
        }

        /// <summary>
        /// Declines a dialog and waits for window disappearance.
        /// </summary>
        public void Decline()
        {
            GetDeclineButton().Click();
            WaitForWindowIsNotDisplayed();
        }

        /// <summary>
        /// Closes a dialog and waits for window disappearance.
        /// </summary>
        public void Close()
        {
            GetCloseButton().Click();
            WaitForWindowIsNotDisplayed();
        }

        /// <summary>
        /// Waits for window loader appearance for 1.5 seconds and then for its disappearance.
        /// </summary>
        /// <param name="timeout">disappearance timeout</param>
        /// <returns></returns>
        /// <exception cref="LoaderTimeoutException">thrown if loader has not disappeared during timeout period</exception>
        public virtual bool WaitForLoading(TimeSpan timeout)
        {
            if (Locators.ContainsKey(DialogElement.Loader))
            {
                new LoaderHandler(
                    () => TryGetChild<WebControl>(Locators[DialogElement.Loader]),
                    () => !TryGetChild<WebControl>(Locators[DialogElement.Loader]))
                .WaitFor(timeout);
            }

            return true;
        }

        /// <summary>
        /// Waits during window wait timeout until window is displayed on screen.
        /// </summary>
        /// <exception cref="TimeoutException">Thrown when window has not appeared</exception>
        public virtual void WaitForWindowIsDisplayed()
        {
            Logger.Instance.Log(LogLevel.Trace, "Waiting for window appearance.");

            new DefaultWait
            {
                Timeout = TimeSpan.FromMinutes(1),
                PollingInterval = TimeSpan.FromMilliseconds(250),
                ErrorMessage = $"Window {Name} has not appeared."
            }
            .Until(() => IsWindowDisplayed());
        }

        /// <summary>
        /// Waits during window wait timeout until window is not displayed on screen.
        /// </summary>
        /// <exception cref="TimeoutException">Thrown when window has not disappeared</exception>
        protected virtual void WaitForWindowIsNotDisplayed()
        {
            Logger.Instance.Log(LogLevel.Trace, "Waiting for window disappearance.");

            var wait = new DefaultWait
            {
                Timeout = TimeSpan.FromMinutes(1),
                PollingInterval = TimeSpan.FromMilliseconds(250),
                ErrorMessage = $"Window {Name} has not disappeared."
            };
            wait.IgnoreExceptionTypes(typeof(ControlNotFoundException));
            wait.Until(() => IsWindowNotDisplayed());
        }

        private bool IsWindowDisplayed() =>
            (Cached || this.Exists()) && !GetAttribute("style").Contains("display: none;") && Visible;

        private bool IsWindowNotDisplayed() =>
            !(Cached || this.Exists()) || (!GetAttribute("style").Contains("display: block;") && !Visible);
    }
}
