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


        protected static TimeSpan ImplicitlyWait = _timeoutDefault;
        private static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);


        public T Find<T>(UICore.Driver.By by, string locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            try
            {
                IWebElement elementToWrap = SearchContext.FindElement(GetLocator(by, locator));
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                return wrapper;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {by} = {locator}");
            }
        }


        public T Find<T>(string name, string alternativeName = "") where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");


            string xPath = $".//*[@name = '{name}' or @id = '{name}'";

            if (!string.IsNullOrEmpty(alternativeName))
                xPath += $" or @name = '{alternativeName}' or @id = '{alternativeName}'";

            xPath += "]";

            try
            {
                IWebElement elementToWrap = SearchContext.FindElement(By.XPath(xPath));
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                return wrapper;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by name = {name} and alternative name = {alternativeName}");
            }
        }


        public IList<T> FindList<T>(UICore.Driver.By by, string locator) where T : IControl
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


        public bool WaitFor<T>(UICore.Driver.By by, string locator, int millisecondsTimeout) where T : IControl
        {
            ((IWebDriver)SearchContext).Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(millisecondsTimeout));

            bool isPresented = true;
            try
            {
                Find<T>(by, locator);
            }
            catch (ControlNotFoundException)
            {
                isPresented = false;
            }

            ((IWebDriver)SearchContext).Manage().Timeouts().ImplicitlyWait(_timeoutDefault);

            return isPresented;
        }


        public bool WaitFor<T>(UICore.Driver.By by, string locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            ((IWebDriver)SearchContext).Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(millisecondsTimeout));

            bool isPresented = true;
            try
            {
                controlInstance = Find<T>(by, locator);
            }
            catch (ControlNotFoundException)
            {
                controlInstance = default(T);
                isPresented = false;
            }

            ((IWebDriver)SearchContext).Manage().Timeouts().ImplicitlyWait(_timeoutDefault);

            return isPresented;
        }


        public T FirstChild<T>() where T : IControl
        {
            throw new NotImplementedException();
        }



        #region "Helpers"

        private By GetLocator(UICore.Driver.By by, string locator)
        {
            switch (by)
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

        #endregion

    }
}
