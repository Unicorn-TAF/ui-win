using System;
using OpenQA.Selenium;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Web.Controls
{
    /// <summary>
    /// Represents basic container for other web controls.
    /// Initialized container also initializes all controls and containers within itself.
    /// </summary>
    public class WebContainer : WebControl, IContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebContainer"/> class.
        /// </summary>
        public WebContainer() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebContainer"/> class with wraps specific <see cref="IWebElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IWebElement"/> instance to wrap</param>
        public WebContainer(IWebElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Clicks button with specified text within the container.
        /// </summary>
        /// <param name="locator">button text</param>
        public virtual void ClickButton(string locator) =>
            Find<WebControl>(ByLocator.Xpath($".//button[. = '{locator}']"))
            .Click();

        /// <summary>
        /// Sets specified text into specified text input within the container.
        /// </summary>
        /// <param name="locator">text input name</param>
        /// <param name="text">text to input</param>
        public virtual void InputText(string locator, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects specified radio button within the container.
        /// </summary>
        /// <param name="locator">radio button name</param>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool SelectRadio(string locator)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets specified checkbox within the container in specified state.
        /// </summary>
        /// <param name="locator">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <returns>true - if state was changed; false - if already in desired state</returns>
        public virtual bool SetCheckbox(string locator, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
