using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UICore.UI;
using Unicorn.UIDesktop.UI.Controls;

namespace Unicorn.UIDesktop.UI
{
    public abstract class GuiContainer : GuiControl, IContainer
    {
        public GuiContainer() : base(){ }

        public GuiContainer(AutomationElement instance) : base(instance) { }

        public void ClickButton(string locator)
        {
            Logger.Instance.Debug($"\tClick '{locator}' button");

            Button button = Find<Button>(locator);
            button.Click();
        }

        public void InputText(string locator, string text)
        {
            Logger.Instance.Debug($"\tInput Text '{text}' to '{locator}' field");

            TextInput edit = Find<TextInput>(locator);
            edit.SendKeys(text);
        }

        public bool SelectRadio(string locator)
        {
            Logger.Instance.Debug($"\tSelect '{locator}' radio button");

            Radio radio = Find<Radio>(locator);
            return radio.Select();
        }

        public bool SetCheckbox(string locator, bool state)
        {
            Logger.Instance.Debug($"\tSet checkbox '{locator}' to '{state}'");

            Checkbox checkbox = Find<Checkbox>(locator);

            if (state)
                return checkbox.Check();
            else
                return checkbox.Uncheck();
        }
    }
}
