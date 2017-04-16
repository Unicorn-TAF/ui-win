using System;
using OpenQA.Selenium;
using UICore.UI;
using UICore.UIProperties;
using UIWeb.Driver;

namespace UIWeb.UI
{
    public class WebControl : WebSearchContext, IControl {

        public IWebElement Instance;


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

        public string Name
        {
            get
            {
                throw new NotImplementedException();
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
