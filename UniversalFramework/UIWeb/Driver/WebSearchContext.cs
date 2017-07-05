using OpenQA.Selenium;
using System;
using Unicorn.UICore.UI;
using Unicorn.UIWeb.UI;

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
            if (typeof(WebControl).IsAssignableFrom(typeof(T)))
            {
                IWebElement elementToWrap = SearchContext.FindElement(By.XPath(WebDriver.TransformXpath(locator)));
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).Instance = elementToWrap;
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                return wrapper;
            }
            else
                throw new ArgumentException("Illegal type of control");
        }
    }
}
