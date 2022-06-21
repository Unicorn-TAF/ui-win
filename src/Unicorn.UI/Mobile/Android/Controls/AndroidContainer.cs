using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Mobile.Android.Controls
{
    /// <summary>
    /// Represents basic container for other android controls with ready for use 
    /// standard methods of interaction with base controls.
    /// </summary>
    public class AndroidContainer : AndroidControl, IContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidContainer"/> class.
        /// </summary>
        public AndroidContainer() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidContainer"/> class 
        /// which wraps specific <see cref="AppiumWebElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AppiumWebElement"/> instance to wrap</param>
        public AndroidContainer(AppiumWebElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Clicks button with specified text.<para/>
        /// </summary>
        /// <param name="name">button text</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void ClickButton(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets specified text into text input with specified placeholder.<para/>
        /// </summary>
        /// <param name="name">text input placeholder text</param>
        /// <param name="text">text to input</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void InputText(string name, string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects specified radio button by label text.<para/>
        /// </summary>
        /// <param name="name">radio button label</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool SelectRadio(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets specified state for checkbox with specified name.<para/>
        /// </summary>
        /// <param name="name">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if state was changed; false - if already in desired state</returns>
        public virtual bool SetCheckbox(string name, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
