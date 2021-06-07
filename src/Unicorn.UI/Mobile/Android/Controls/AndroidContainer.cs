using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Mobile.Android.Controls
{
    public class AndroidContainer : AndroidControl, IContainer
    {
        public AndroidContainer() : base()
        {
        }

        public AndroidContainer(AppiumWebElement instance) : base(instance)
        {
        }

        public void ClickButton(string name)
        {
            throw new NotImplementedException();
        }

        public void InputText(string name, string text)
        {
            throw new NotImplementedException();
        }

        public bool SelectRadio(string name)
        {
            throw new NotImplementedException();
        }

        public bool SetCheckbox(string name, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
