using System;
using System.Windows.Automation;
using UICore.UI;
using UIDesktop.UI.Controls;

namespace UIDesktop.UI
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
