using System;
using System.Drawing;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.IOS.Driver;

namespace Unicorn.UI.Mobile.IOS.Controls
{
    public class IOSControl : IOSSearchContext, IControl
    {
        public bool Cached { get; set; } = true;

        public ByLocator Locator { get; set; }

        public string Name { get; set; }

        public AppiumWebElement Instance
        {
            get
            {
                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
            }
        }

        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Enabled
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Text
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Point Location
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override AppiumWebElement SearchContext
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
            }
        }

        public void SendKeys(string keys)
        {
            this.Instance.SendKeys(keys);
        }

        public void Click()
        {
            this.Instance.Click();
        }

        public void RightClick()
        {
            throw new NotImplementedException();
        }

        public string GetAttribute(string attribute)
        {
            throw new NotImplementedException();
        }
    }
}
