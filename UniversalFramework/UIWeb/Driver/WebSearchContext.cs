using OpenQA.Selenium;
using System;
using Unicorn.UICore.UI;
using Unicorn.UIWeb.UI;
using System.Collections.Generic;

namespace Unicorn.UIWeb.Driver
{
    public abstract class WebSearchContext : UICore.Driver.ISearchContext
    {
        protected ISearchContext SearchContext;

        protected TimeSpan ImplicitlyWait = TimeSpan.FromSeconds(20);


        public T FindControl<T>(UICore.Driver.By by, string locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");


            IWebElement elementToWrap = SearchContext.FindElement(GetLocator(by, locator));
            var wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).SearchContext = elementToWrap;
            return wrapper;
                
        }

        public IList<T> FindControls<T>(UICore.Driver.By by, string locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            List<T> listElements = new List<T>();
            
            IList<IWebElement> elementsToWrap = SearchContext.FindElements(GetLocator(by, locator));

            foreach (IWebElement elementToWrap in elementsToWrap)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                listElements.Add(wrapper);
            }

            return listElements;
        }


        private By GetLocator(UICore.Driver.By by, string locator)
        {
            switch(by)
            {
                case UICore.Driver.By.Web_Xpath:
                    return By.XPath(WebDriver.TransformXpath(locator));
                case UICore.Driver.By.Web_Css:
                    return By.CssSelector(locator);
                case UICore.Driver.By.Id:
                    return By.Id(locator);
                case UICore.Driver.By.Name:
                    return By.Name(locator);
                case UICore.Driver.By.Class:
                    return By.ClassName(locator);
                case UICore.Driver.By.Web_Tag:
                    return By.TagName(locator);
                default:
                    throw new ArgumentException($"Incorrect locator type specified:  {by}");
            }
        }
    }
}
