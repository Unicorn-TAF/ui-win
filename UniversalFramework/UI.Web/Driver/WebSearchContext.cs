using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    public abstract class WebSearchContext : Core.Driver.ISearchContext
    {
        public OpenQA.Selenium.ISearchContext ParentContext;

        protected static TimeSpan implicitlyWaitTimeout = TimeoutDefault;

        protected static TimeSpan TimeoutDefault => TimeSpan.FromSeconds(20);

        protected virtual OpenQA.Selenium.ISearchContext SearchContext { get; set; }

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
            WebDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);

            bool isPresented = true;
            try
            {
                Find<T>(locator);
            }
            catch (ControlNotFoundException)
            {
                isPresented = false;
            }

            WebDriver.Instance.ImplicitlyWait = TimeoutDefault;

            return isPresented;
        }

        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            WebDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);

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

            WebDriver.Instance.ImplicitlyWait = TimeoutDefault;

            return isPresented;
        }

        public T FirstChild<T>() where T : IControl
        {
            throw new NotImplementedException();
        }

        #region "Helpers"

        protected IWebElement GetNativeControl(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            try
            {
                IWebElement nativeControl = this.SearchContext.FindElement(by);
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
                IWebElement nativeControl = this.ParentContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }

        private IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Illegal type of control: " + typeof(T));
            }

            List<T> controlsList = new List<T>();
            IList<IWebElement> wrappedElements = GetNativeControlsList(locator);

            foreach (IWebElement wrappedElement in wrappedElements)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((WebControl)(object)wrapper).Instance = wrappedElement;
                ((WebControl)(object)wrapper).ParentContext = this.SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        private T GetWrappedControl<T>(ByLocator locator)
        {
            if (!typeof(WebControl).IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException("Illegal type of control: " + typeof(T));
            }

            IWebElement elementToWrap = GetNativeControl(locator);
            var wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).Instance = elementToWrap;
            ((WebControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }

        private IList<IWebElement> GetNativeControlsList(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            
            try
            {
                IList<IWebElement> nativeControls = this.SearchContext.FindElements(by);
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
                case Using.Web_Xpath:
                    return By.XPath(/*WebDriver.TransformXpath(*/locator.Locator);
                case Using.Web_Css:
                    return By.CssSelector(locator.Locator);
                case Using.Id:
                    return By.Id(locator.Locator);
                case Using.Name:
                    return By.Name(locator.Locator);
                case Using.Class:
                    return By.ClassName(locator.Locator);
                case Using.Web_Tag:
                    return By.TagName(locator.Locator);
                default:
                    throw new ArgumentException($"Incorrect locator type specified:  {locator.How}");
            }
        }

        #endregion
    }
}
