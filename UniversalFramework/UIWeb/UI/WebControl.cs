using System;
using OpenQA.Selenium;
using Unicorn.UICore.UI;
using Unicorn.UICore.UIProperties;
using Unicorn.UIWeb.Driver;

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
                throw new NotImplementedException();
            }
        }

        public UIPoint Location
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
                return Instance.Text;
            }
        }

        public UISize Size
        {
            get
            {
                throw new NotImplementedException();
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
