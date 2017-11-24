using System;
using System.Reflection;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.UI.Controls;

namespace Unicorn.UI.Desktop.UI
{
    public abstract class GuiContainer : GuiControl, IContainer
    {
        public override AutomationElement Instance
        {
            get
            {
                if (!Cached)
                    SearchContext = GetNativeControlFromParentContext(Locator, GetType());

                return SearchContext;
            }
            set
            {
                SearchContext = value;
                Init();
            }
        }


        public GuiContainer() : base(){ }

        public GuiContainer(AutomationElement instance) : base(instance)
        { }



        public void Init()
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach(FieldInfo field in fields)
            {
                object[] attributes = field.GetCustomAttributes(typeof(FindAttribute), true);
                if (attributes.Length != 0)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);
                    ((GuiControl)control).Locator = ((FindAttribute)attributes[0]).Locator;
                    ((GuiControl)control).Cached = false;
                    ((GuiControl)control).ParentContext = SearchContext;
                    field.SetValue(this, control);
                }
            }
        }


        public void ClickButton(string locator)
        {
            Logger.Instance.Debug($"\tClick '{locator}' button");

            Button button = Find<Button>(ByLocator.Name(locator));
            button.Click();
        }

        public void InputText(string locator, string text)
        {
            Logger.Instance.Debug($"\tInput Text '{text}' to '{locator}' field");

            TextInput edit = Find<TextInput>(ByLocator.Name(locator));
            edit.SendKeys(text);
        }

        public bool SelectRadio(string locator)
        {
            Logger.Instance.Debug($"\tSelect '{locator}' radio button");

            Radio radio = Find<Radio>(ByLocator.Name(locator));
            return radio.Select();
        }

        public bool SetCheckbox(string locator, bool state)
        {
            Logger.Instance.Debug($"\tSet checkbox '{locator}' to '{state}'");

            Checkbox checkbox = Find<Checkbox>(ByLocator.Name(locator));

            if (state)
                return checkbox.Check();
            else
                return checkbox.Uncheck();
        }
    }
}
