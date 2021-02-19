using OpenQA.Selenium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Web.Controls.Typified;

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
        /// Clicks button with specified text.<para/>
        /// Default locator is: .//button[normalize-space() = 'BUTTON_TEXT']
        /// </summary>
        /// <param name="name">button text</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void ClickButton(string name) =>
            Find<WebControl>(ByLocator.Xpath($".//button[normalize-space() = '{name}']"))
            .Click();

        /// <summary>
        /// Sets specified text into text input with specified placeholder.<para/>
        /// Default locator is: .//input[@placeholder='INPUT_PLACEHOLDER']
        /// </summary>
        /// <param name="name">text input placeholder text</param>
        /// <param name="text">text to input</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void InputText(string name, string text) =>
            Find<TextInput>(ByLocator.Xpath($".//input[@placeholder='{name}']"))
            .SetValue(text);

        /// <summary>
        /// Selects specified radio button by label text.<para/>
        /// Default locator is: .//input[@type='radio' and ..//*[. = 'RADIO_LABEL_TEXT']]
        /// </summary>
        /// <param name="name">radio button label</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool SelectRadio(string name) =>
            Find<Radio>(ByLocator.Xpath($".//input[@type='radio' and ..//*[. = '{name}']]"))
            .Select();

        /// <summary>
        /// Sets specified state for checkbox with specified name.<para/>
        /// Default locator is: .//input[@type='checkbox' and @name='CHECKBOX_NAME']
        /// </summary>
        /// <param name="name">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if state was changed; false - if already in desired state</returns>
        public virtual bool SetCheckbox(string name, bool state) =>
            Find<Checkbox>(ByLocator.Xpath($".//input[@type='checkbox' and @name='{name}']"))
            .SetCheckedState(state);
    }
}
