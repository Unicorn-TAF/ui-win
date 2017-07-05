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

        public void ClickButton(string buttonName)
        {
            Button button = GetElement<Button>(buttonName);
            button.WaitForEnabled();
            button.Click();
        }

        public bool InputText(string inputName, string text)
        {
            TextInput edit = GetElement<TextInput>(inputName);

            if (edit.Value.Equals(text))
                return false;
            else
            {
                edit.WaitForEnabled();
                edit.SendKeys(text);
                return true;
            }
        }

        public bool SelectRadio(string radioLabel)
        {
            throw new NotImplementedException();
        }

        public bool SetheckboxState(string checkboxLabel, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
