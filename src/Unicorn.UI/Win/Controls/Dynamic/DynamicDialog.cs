using System;
using System.Collections.Generic;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.PageObject;

namespace Unicorn.UI.Win.Controls.Dynamic
{
    /// <summary>
    /// Describes dynamically defined Dialog window (each sub-control could be defined using attribute).
    /// </summary>
    public class DynamicDialog : WinContainer, IDynamicDialog
    {
        /// <summary>
        /// Gets control for dialog title.
        /// </summary>
        [Name("Window title")]
        public virtual IControl TitleControl => Locators.ContainsKey(DialogElement.Title) ? 
            Find<WinControl>(Locators[DialogElement.Title]) :
            throw new NotSpecifiedLocatorException($"{nameof(TitleControl)} dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog content.
        /// </summary>
        [Name("Window content")]
        public virtual IControl ContentControl => Locators.ContainsKey(DialogElement.Content) ?
            Find<WinControl>(Locators[DialogElement.Content]) :
            throw new NotSpecifiedLocatorException($"{nameof(ContentControl)} dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog acceptance button.
        /// </summary>
        [Name("Accept button")]
        public virtual IControl AcceptButton => Locators.ContainsKey(DialogElement.Accept) ?
            Find<WinControl>(Locators[DialogElement.Accept]) :
            throw new NotSpecifiedLocatorException($"{nameof(AcceptButton)} dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog declining button.
        /// </summary>
        [Name("Decline button")]
        public virtual IControl DeclineButton => Locators.ContainsKey(DialogElement.Decline) ?
            Find<WinControl>(Locators[DialogElement.Decline]) :
            throw new NotSpecifiedLocatorException($"{nameof(DeclineButton)} dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets control for dialog close button.
        /// </summary>
        [Name("Cancel button")]
        public virtual IControl CloseButton => Locators.ContainsKey(DialogElement.Close) ?
            Find<Button>(Locators[DialogElement.Close]) :
            throw new NotSpecifiedLocatorException($"{nameof(CloseButton)} dialog sub-control locator is not specified.");

        /// <summary>
        /// Gets dialog title text.
        /// </summary>
        public virtual string Title => TitleControl.Text.Trim();

        /// <summary>
        /// Gets dialog text content.
        /// </summary>
        public virtual string TextContent => ContentControl.Text.Trim();

        /// <summary>
        /// Gets dictionary of dd elements locators.
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

        public void Accept()
        {
            AcceptButton.Click();
            WaitForWindowIsNotDisplayed();
        }

        public void Decline()
        {
            DeclineButton.Click();
            WaitForWindowIsNotDisplayed();
        }

        public void Close()
        {
            CloseButton.Click();
            WaitForWindowIsNotDisplayed();
        }

        /// <summary>
        /// Waits for window loader appearance for 1.5 seconds and then for its disappearance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="LoaderTimeoutException">thrown if loader has not disappeared during timeout period</exception>
        public virtual bool WaitForLoading(TimeSpan timeout)
        {
            if (Locators.ContainsKey(DialogElement.Loader))
            {
                new LoaderHandler(
                    () => TryGetChild<WinControl>(Locators[DialogElement.Loader]),
                    () => !TryGetChild<WinControl>(Locators[DialogElement.Loader]))
                .WaitFor(timeout);
            }

            return true;
        }

        /// <summary>
        /// Waits during window wait timeout until window is displayed on screen.
        /// </summary>
        /// <param name="waitForContentLoading">true - if need to wait for content loading, otherwise - false</param>
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
            (Cached || this.Exists()) && Visible;

        protected bool IsWindowNotDisplayed() =>
            !(Cached || this.Exists()) || !Visible;
    }
}
