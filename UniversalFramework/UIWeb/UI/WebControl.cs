using System;
using OpenQA.Selenium;
using Unicorn.UICore.UI;
using Unicorn.UIWeb.Driver;
using System.Drawing;
using OpenQA.Selenium.Interactions;

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

        public string Text
        {
            get
            {
                return Instance.Text;
            }
        }

        public bool Enabled
        {
            get
            {
                return Instance.Enabled;
            }
        }

        public bool Visible
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
                return Instance.Location;
            }
        }

        public Size Size
        {
            get
            {
                return Instance.Size;
            }
        }



        public string GetAttribute(string attribute)
        {
            return Instance.GetAttribute(attribute);
        }


        public void Click()
        {
            Instance.Click();
        }

        public void RightClick()
        {
            Actions actions = new Actions((IWebDriver)SearchContext);
            actions.MoveToElement(Instance);
            actions.ContextClick();
            actions.Release().Perform();
        }
    }
}
