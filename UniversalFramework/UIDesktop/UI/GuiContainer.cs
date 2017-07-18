using System;
using System.Windows.Automation;
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
            Button button = Find<Button>(locator);
            button.Click();
        }

        public void InputText(string locator, string text)
        {
            TextInput edit = Find<TextInput>(locator);
            edit.SendKeys(text);
        }

        public bool SelectRadio(string locator)
        {
            Radio radio = Find<Radio>(locator);
            return radio.Select();
        }

        public bool SetCheckbox(string locator, bool state)
        {
            Checkbox checkbox = Find<Checkbox>(locator);


            if (state)
                return checkbox.Check();
            else
                return checkbox.Uncheck();
        }
    }
}
