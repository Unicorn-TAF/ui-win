using UIAutomationClient;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Win.Controls.Typified;

namespace Unicorn.UI.Win.Controls
{
    /// <summary>
    /// Represents basic container for other windows controls.
    /// Initialized container also initializes all controls and containers within itself.
    /// </summary>
    public class WinContainer : WinControl, IContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinContainer"/> class.
        /// </summary>
        protected WinContainer() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WinContainer"/> class with wraps specific <see cref="IUIAutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="IUIAutomationElement"/> instance to wrap</param>
        protected WinContainer(IUIAutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Clicks button with specified name within the container.
        /// </summary>
        /// <param name="name">button name</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void ClickButton(string name)
        {
            ULog.Debug("Click '{0}' button", name);
            Find<Button>(ByLocator.Name(name)).Click();
        }

        /// <summary>
        /// Sets specified text into specified text input within the container.
        /// </summary>
        /// <param name="name">text input name</param>
        /// <param name="text">text to input</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void InputText(string name, string text)
        {
            ULog.Debug("Input Text '{0}' to '{1}' field", text, name);
            Find<TextInput>(ByLocator.Name(name)).SetValue(text);
        }

        /// <summary>
        /// Selects specified radio button within the container.
        /// </summary>
        /// <param name="name">radio button name</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool SelectRadio(string name)
        {
            ULog.Debug("Select '{0}' radio button", name);
            return Find<Radio>(ByLocator.Name(name)).Select();
        }

        /// <summary>
        /// Sets specified checkbox within the container in specified state.
        /// </summary>
        /// <param name="name">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        /// <returns>true - if state was changed; false - if already in desired state</returns>
        public virtual bool SetCheckbox(string name, bool state)
        {
            ULog.Debug("Set checkbox '{0}' to '{1}'", name, state);
            return Find<Checkbox>(ByLocator.Name(name)).SetCheckedState(state);
        }
    }
}
