using Appium.Interfaces.Generic.SearchContext;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using System;
using System.Collections.Generic;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.Controls;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidSearchContext : Core.Driver.ISearchContext
    {
        public AppiumWebElement ParentContext;
        protected virtual AppiumWebElement SearchContext { get; set; }


        protected static TimeSpan ImplicitlyWaitTimeout = _timeoutDefault;
        protected static TimeSpan _timeoutDefault = TimeSpan.FromSeconds(20);


        public T Find<T>(ByLocator locator) where T : IControl
        {
            return GetWrappedControl<T>(locator);
        }


        public IList<T> FindList<T>(ByLocator locator) where T : IControl
        {
            return GetWrappedControlsList<T>(locator);
        }


        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout) where T : IControl
        {
            AndroidDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);

            bool isPresented = true;
            try
            {
                Find<T>(locator);
            }
            catch (ControlNotFoundException)
            {
                isPresented = false;
            }

            AndroidDriver.Instance.ImplicitlyWait =_timeoutDefault;

            return isPresented;
        }


        public bool WaitFor<T>(ByLocator locator, int millisecondsTimeout, out T controlInstance) where T : IControl
        {
            AndroidDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(millisecondsTimeout);

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

            AndroidDriver.Instance.ImplicitlyWait = _timeoutDefault;

            return isPresented;
        }


        public T FirstChild<T>() where T : IControl
        {
            throw new NotImplementedException();
        }



        #region "Helpers"

        private IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            if (!typeof(AndroidControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            List<T> controlsList = new List<T>();
            IList<AppiumWebElement> wElements = GetNativeControlsList(locator);

            foreach (AppiumWebElement wElement in wElements)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((AndroidControl)(object)wrapper).Instance = wElement;
                ((AndroidControl)(object)wrapper).ParentContext = SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        private T GetWrappedControl<T>(ByLocator locator)
        {
            if (!typeof(AndroidControl).IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Illegal type of control: " + typeof(T));

            AppiumWebElement elementToWrap = GetNativeControl(locator);
            var wrapper = Activator.CreateInstance<T>();
            ((AndroidControl)(object)wrapper).Instance = elementToWrap;
            ((AndroidControl)(object)wrapper).ParentContext = SearchContext;

            return wrapper;
        }

        protected AppiumWebElement GetNativeControl(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            try
            {
                AppiumWebElement nativeControl = SearchContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }


        protected AppiumWebElement GetNativeControlFromParentContext(ByLocator locator)
        {
            By by = GetNativeLocator(locator);
            try
            {
                AppiumWebElement nativeControl = ParentContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }


        private IList<AppiumWebElement> GetNativeControlsList(ByLocator locator)
        {
            By by = GetNativeLocator(locator);

            try
            {
                IList<AppiumWebElement> nativeControls = SearchContext.FindElements(by);
                return nativeControls;
            }
            catch (NoSuchElementException)
            {
                return new List<AppiumWebElement>();
            }
        }


        private By GetNativeLocator(ByLocator locator)
        {
            switch (locator.How)
            {
                case Using.Web_Xpath:
                    return By.XPath(locator.Locator);
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
