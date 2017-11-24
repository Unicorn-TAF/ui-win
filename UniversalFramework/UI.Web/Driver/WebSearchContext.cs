using OpenQA.Selenium;
using System;
using Unicorn.UI.Core.UI;
using Unicorn.UIWeb.UI;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UIWeb.Driver
{
    public abstract class WebSearchContext : Unicorn.UI.Core.Driver.ISearchContext
    {
        protected OpenQA.Selenium.ISearchContext ParentContext;
        protected OpenQA.Selenium.ISearchContext SearchContext;


        protected static TimeSpan ImplicitlyWait = _timeoutDefault;
        protected static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);


        public T Find<T>(ByLocator locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            try
            {
                IWebElement elementToWrap = SearchContext.FindElement(GetLocator(locator));
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                return wrapper;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
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


        public IList<T> FindList<T>(ByLocator locator) where T : IControl
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control");

            List<T> listElements = new List<T>();
            
            IList<IWebElement> elementsToWrap = SearchContext.FindElements(GetLocator(locator));

            foreach (IWebElement elementToWrap in elementsToWrap)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).SearchContext = elementToWrap;
                listElements.Add(wrapper);
            }

            return listElements;
        }


        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout) where T : IControl
        {
            WebDriver.Instance.SetImplicitlyWait(TimeSpan.FromMilliseconds(millisecondsTimeout));

            bool isPresented = true;
            try
            {
                Find<T>(locator);
            }
            catch (ControlNotFoundException)
            {
                isPresented = false;
            }

            WebDriver.Instance.SetImplicitlyWait(_timeoutDefault);

            return isPresented;
        }


        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            WebDriver.Instance.SetImplicitlyWait(TimeSpan.FromMilliseconds(millisecondsTimeout));

            bool isPresented = true;
            try
            {
                controlInstance = Find<T>(locator);
            }
            catch (ControlNotFoundException)
            {
                controlInstance = default(T);
                isPresented = false;
            }

            WebDriver.Instance.SetImplicitlyWait(_timeoutDefault);

            return isPresented;
        }


        public T FirstChild<T>() where T : IControl
        {
            throw new NotImplementedException();
        }



        #region "Helpers"

        private By GetLocator(ByLocator locator)
        {
            switch (locator.How)
            {
                case LocatorType.Web_Xpath:
                    return By.XPath(WebDriver.TransformXpath(locator.Locator));
                case LocatorType.Web_Css:
                    return By.CssSelector(locator.Locator);
                case LocatorType.Id:
                    return By.Id(locator.Locator);
                case LocatorType.Name:
                    return By.Name(locator.Locator);
                case LocatorType.Class:
                    return By.ClassName(locator.Locator);
                case LocatorType.Web_Tag:
                    return By.TagName(locator.Locator);
                default:
                    throw new ArgumentException($"Incorrect locator type specified:  {locator.How}");
            }
        }

        #endregion

    }
}
