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
            throw new NotImplementedException();
        }

        public bool SetheckboxState(string locator, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
