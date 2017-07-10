using System;
using OpenQA.Selenium;
using Unicorn.UICore.UI;
using Unicorn.UIWeb.Driver;
using System.Drawing;

namespace Unicorn.UIWeb.UI
{
    public class WebControl : WebSearchContext, IControl {

        public IWebElement Instance
        {
            get
            {
                return (IWebElement)SearchContext;
            }
        }


        public string GetAttribute(string attribute)
        {
            return Instance.GetAttribute(attribute);
        }

        public bool Enabled
        {
            get
            {
                return Instance.Enabled;
            }
        }

        public Point Location
        {
            get
            {
                return Instance.Location;
            }
        }

        public string Text
        {
            get
            {
                return Instance.Text;
            }
        }

        public Size Size
        {
            get
            {
                return Instance.Size;
            }
        }

        public bool Visible
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void CheckAttributeContains(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void CheckAttributeDoeNotContain(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void CheckAttributeEquals(string attribute, string expectedValue)
        {
            throw new NotImplementedException();
        }

        public void Click()
        {
            Instance.Click();
        }

        public void WaitForAttributeValue(string attribute, string value, bool contains = true)
        {
            throw new NotImplementedException();
        }

        public void WaitForEnabled()
        {
            throw new NotImplementedException();
        }
    }
}
