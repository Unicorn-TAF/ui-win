using OpenQA.Selenium.Appium;
using System;
using System.Drawing;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.iOS.Driver;

namespace Unicorn.UI.Mobile.iOS.Controls
{
    public class IOSControl : iOSSearchContext, IControl
    {
        public bool Cached = true;

        public ByLocator Locator { get; set; }

        public AppiumWebElement Instance
        {
            get
            {
                return SearchContext;
            }

            set
            {
                SearchContext = value;
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
                    base.SearchContext = GetNativeControlFromParentContext(Locator);
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
            Instance.SendKeys(keys);
        }

        public void Click()
        {
            Instance.Click();
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
