using System;
using System.Reflection;
using OpenQA.Selenium;
using Unicorn.UI.Core.PageObject;

namespace Unicorn.UI.Web.Controls
{
    public class WebContainer : WebControl, IContainer
    {
        public WebContainer() : base()
        {
        }

        public WebContainer(IWebElement instance) : base(instance)
        {
        }

        protected override ISearchContext SearchContext
        {
            get
            {
                if (!this.Cached)
                {
                    base.SearchContext = GetNativeControlFromParentContext(this.Locator);
                }

                return base.SearchContext;
            }

            set
            {
                base.SearchContext = value;
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
                    ((WebControl)control).Locator = (attributes[0]).Locator;
                    ((WebControl)control).Cached = false;
                    ((WebControl)control).ParentContext = this.SearchContext;

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((WebControl)control).Name = nameAttribute.Name;
                    }

                    if (controlType.IsSubclassOf(typeof(WebContainer)))
                    {
                        ((WebContainer)control).Init();
                    }
                    
                    field.SetValue(this, control);
                }
            }
        }

        public virtual void InputText(string locator, string text)
        {
            throw new NotImplementedException();
        }

        public virtual bool SelectRadio(string locator)
        {
            throw new NotImplementedException();
        }

        public virtual bool SetCheckbox(string locator, bool state)
        {
            throw new NotImplementedException();
        }

        public virtual void ClickButton(string locator)
        {
            throw new NotImplementedException();
        }
    }
}
