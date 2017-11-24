using OpenQA.Selenium;
using System;
using Unicorn.UI.Core.UI;
using Unicorn.UI.Web.UI;
using System.Collections.Generic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Web.Driver
{
    public abstract class WebSearchContext : Core.Driver.ISearchContext
    {
        public OpenQA.Selenium.ISearchContext ParentContext;
        protected OpenQA.Selenium.ISearchContext SearchContext;


        protected static TimeSpan ImplicitlyWait = _timeoutDefault;
        protected static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);


        public T Find<T>(ByLocator locator) where T : IControl
        {
            return GetWrappedControl<T>(locator);
        }

        /*
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
        */

        public IList<T> FindList<T>(ByLocator locator) where T : IControl
        {
            return GetWrappedControlsList<T>(locator);
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

        private IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            List<T> controlsList = new List<T>();
            IList<IWebElement> wElements = GetNativeControlsList(locator);

            foreach (IWebElement wElement in wElements)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).Instance = wElement;
                ((WebControl)(object)wrapper).ParentContext = SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        private T GetWrappedControl<T>(ByLocator locator)
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            IWebElement elementToWrap = GetNativeControl(locator);
            var wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).Instance = elementToWrap;
            ((WebControl)(object)wrapper).ParentContext = SearchContext;

            return wrapper;
        }

        protected IWebElement GetNativeControl(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            try
            {
                IWebElement nativeControl = SearchContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }


        protected IWebElement GetNativeControlFromParentContext(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            try
            {
                IWebElement nativeControl = ParentContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }


        private IList<IWebElement> GetNativeControlsList(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            
            try
            {
                IList<IWebElement> nativeControls = SearchContext.FindElements(by);
                return nativeControls;
            }
            catch (NoSuchElementException)
            {
                return new List<IWebElement>();
            }
        }


        private By GetNativeLocator(ByLocator locator)
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
