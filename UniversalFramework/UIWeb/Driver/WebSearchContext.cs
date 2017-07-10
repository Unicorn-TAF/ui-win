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


        public T GetElement<T>(string locator) where T : IControl
        {
            WebDriver.Instance.SetImplicitlyWait(TimeSpan.FromSeconds(0));
            T el = WaitForElement<T>(locator);
            WebDriver.Instance.SetImplicitlyWait(TimeSpan.FromSeconds(20));
            return el;
        }

        public T WaitForElement<T>(string locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            IWebElement elementToWrap = SearchContext.FindElement(By.XPath(WebDriver.TransformXpath(locator)));
            var wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).SearchContext = elementToWrap;
            return wrapper;
                
        }

        public IList<T> FindControls<T>(string locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            List<T> listElements = new List<T>();
            
            IList<IWebElement> elementsToWrap = SearchContext.FindElements(By.XPath(WebDriver.TransformXpath(locator)));

            foreach (IWebElement elementToWrap in elementsToWrap)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                listElements.Add(wrapper);
            }

            return listElements;
        }
    }
}
