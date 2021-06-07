using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UI.Desktop.Controls
{
    /// <summary>
    /// Represents basic container for other windows controls.
    /// Initialized container also initializes all controls and containers within itself.
    /// </summary>
    public abstract class GuiContainer : GuiControl, IContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GuiContainer"/> class.
        /// </summary>
        protected GuiContainer() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuiContainer"/> class with wraps specific <see cref="AutomationElement"/>
        /// </summary>
        /// <param name="instance"><see cref="AutomationElement"/> instance to wrap</param>
        protected GuiContainer(AutomationElement instance) : base(instance)
        {
        }

        /// <summary>
        /// Clicks button with specified name within the container.
        /// </summary>
        /// <param name="name">button name</param>
        /// <exception cref="ControlNotFoundException">is thrown when control was not found</exception>
        public virtual void ClickButton(string name)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Click '{name}' button");
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
            Logger.Instance.Log(LogLevel.Debug, $"Input Text '{text}' to '{name}' field");
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
            Logger.Instance.Log(LogLevel.Debug, $"Select '{name}' radio button");
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
            Logger.Instance.Log(LogLevel.Debug, $"Set checkbox '{name}' to '{state}'");
            return Find<Checkbox>(ByLocator.Name(name)).SetCheckedState(state);
        }
    }
}
