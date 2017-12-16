using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.IOS.Controls;

namespace Unicorn.UI.Mobile.IOS.Driver
{
    public class IOSSearchContext : UISearchContext
    {
        public AppiumWebElement ParentContext { get; set; }

        protected virtual AppiumWebElement SearchContext { get; set; }

        protected override Type ControlsBaseType => typeof(IOSControl);

        #region "Helpers"

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            CheckForControlType<T>();

            AppiumWebElement elementToWrap = GetNativeControl(locator);

            T wrapper = Activator.CreateInstance<T>();
            ((IOSControl)(object)wrapper).Instance = elementToWrap;
            ((IOSControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            CheckForControlType<T>();

            var elementsToWrap = GetNativeControlsList(locator);

            List<T> controlsList = new List<T>();

            foreach (var elementToWrap in elementsToWrap)
            {
                var wrapper = Activator.CreateInstance<T>();
                ((IOSControl)(object)wrapper).Instance = elementToWrap;
                ((IOSControl)(object)wrapper).ParentContext = this.SearchContext;
                controlsList.Add(wrapper);
            }

            return controlsList;
        }

        protected AppiumWebElement GetNativeControl(ByLocator locator)
        {
            By by = GetNativeLocator(locator);

            try
            {
                AppiumWebElement nativeControl = this.SearchContext.FindElement(by);
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
                AppiumWebElement nativeControl = this.ParentContext.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            iOSDriver.Instance.ImplicitlyWait = timeout;
        }

        private IList<AppiumWebElement> GetNativeControlsList(ByLocator locator)
        {
            By by = GetNativeLocator(locator);

            try
            {
                IList<AppiumWebElement> nativeControls = this.SearchContext.FindElements(by);
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
