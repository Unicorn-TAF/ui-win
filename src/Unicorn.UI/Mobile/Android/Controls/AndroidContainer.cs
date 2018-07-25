using OpenQA.Selenium.Appium;
using System;
using System.Reflection;
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

        public override AppiumWebElement Instance
        {
            get
            {
                if (!this.Cached)
                {
                    this.SearchContext = GetNativeControlFromParentContext(this.Locator);
                }

                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
                this.Init();
            }
        }

        public void Init()
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(FindAttribute), true) as FindAttribute[];
                if (attributes.Length != 0)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);
                    ((AndroidControl)control).Locator = attributes[0].Locator;
                    ((AndroidControl)control).Cached = false;
                    ((AndroidControl)control).ParentSearchContext = this;

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((AndroidControl)control).Name = nameAttribute.Name;
                    }

                    if (controlType.IsSubclassOf(typeof(AndroidContainer)))
                    {
                        ((AndroidContainer)control).Init();
                    }

                    field.SetValue(this, control);
                }
            }
        }

        public void ClickButton(string locator)
        {
            throw new NotImplementedException();
        }

        public void InputText(string locator, string text)
        {
            throw new NotImplementedException();
        }

        public bool SelectRadio(string locator)
        {
            throw new NotImplementedException();
        }

        public bool SetCheckbox(string locator, bool state)
        {
            throw new NotImplementedException();
        }
    }
}
