using System.Windows.Automation;
using Unicorn.Taf.Core.Logging;
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
        /// <param name="locator">button name</param>
        public virtual void ClickButton(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Click '{locator}' button");
            Find<Button>(ByLocator.Name(locator)).Click();
        }

        /// <summary>
        /// Sets specified text into specified text input within the container.
        /// </summary>
        /// <param name="locator">text input name</param>
        /// <param name="text">text to input</param>
        public virtual void InputText(string locator, string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Input Text '{text}' to '{locator}' field");
            Find<TextInput>(ByLocator.Name(locator)).SetValue(text);
        }

        /// <summary>
        /// Selects specified radio button within the container.
        /// </summary>
        /// <param name="locator">radio button name</param>
        /// <returns>true - if selection was made; false - if already selected</returns>
        public virtual bool SelectRadio(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{locator}' radio button");
            return Find<Radio>(ByLocator.Name(locator)).Select();
        }

        /// <summary>
        /// Sets specified checkbox within the container in specified state.
        /// </summary>
        /// <param name="locator">checkbox name</param>
        /// <param name="state">state to set for checkbox</param>
        /// <returns>true - if state was changed; false - if already in desired state</returns>
        public virtual bool SetCheckbox(string locator, bool state)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set checkbox '{locator}' to '{state}'");
            return Find<Checkbox>(ByLocator.Name(locator)).SetCheckedState(state);
        }
    }
}
