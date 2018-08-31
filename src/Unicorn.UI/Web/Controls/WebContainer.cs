using System;
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

        public override IWebElement Instance
        {
            get
            {
                if (!this.Cached)
                {
                    this.SearchContext = GetNativeControlFromParentContext(this.Locator);
                }

                return (IWebElement)this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
                ContainerFactory.InitContainer(this);
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
