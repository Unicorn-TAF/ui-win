using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Base.Driver
{
    public abstract class MobileSearchContext : BaseSearchContext<MobileSearchContext>
    {
        protected virtual AppiumWebElement SearchContext { get; set; }

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            AppiumWebElement elementToWrap = GetNativeControl(locator);
            return Wrap<T>(elementToWrap, locator);
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList(locator);
            List<T> controlsList = new List<T>();

            foreach (var elementToWrap in elementsToWrap)
            {
                controlsList.Add(Wrap<T>(elementToWrap, null));
            }

            return controlsList;
        }

        protected override T GetFirstChildWrappedControl<T>()
        {
            var elementToWrap = GetNativeControlsList(new ByLocator(Using.WebXpath, "./*"))[0];
            return Wrap<T>(elementToWrap, null);
        }

        protected AppiumWebElement GetNativeControl(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, SearchContext);
        }

        protected AppiumWebElement GetNativeControlFromParentContext(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, ParentSearchContext.SearchContext);
        }

        protected IList<AppiumWebElement> GetNativeControlsList(ByLocator locator)
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

        protected By GetNativeLocator(ByLocator locator)
        {
            switch (locator.How)
            {
                case Using.WebXpath:
                    return By.XPath(locator.Locator);
                case Using.WebCss:
                    return By.CssSelector(locator.Locator);
                case Using.Id:
                    return By.Id(locator.Locator);
                case Using.Name:
                    return By.Name(locator.Locator);
                case Using.Class:
                    return By.ClassName(locator.Locator);
                case Using.WebTag:
                    return By.TagName(locator.Locator);
                default:
                    throw new ArgumentException($"Incorrect locator type specified:  {locator.How}");
            }
        }

        protected abstract T Wrap<T>(AppiumWebElement elementToWrap, ByLocator locator);

        private AppiumWebElement GetNativeControlFromContext(ByLocator locator, AppiumWebElement context)
        {
            By by = GetNativeLocator(locator);
            try
            {
                AppiumWebElement nativeControl = context.FindElement(by);
                return nativeControl;
            }
            catch (NoSuchElementException)
            {
                throw new ControlNotFoundException($"Unable to find control by {locator}");
            }
        }
    }
}
