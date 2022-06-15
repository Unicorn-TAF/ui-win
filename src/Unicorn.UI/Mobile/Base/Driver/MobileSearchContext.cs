using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Base.Driver
{
    /// <summary>
    /// Describes search context base for mobile controls. 
    /// Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public abstract class MobileSearchContext : BaseSearchContext<MobileSearchContext>
    {
        /// <summary>
        /// Gets or sets Current search context as <see cref="AppiumWebElement"/>
        /// </summary>
        protected virtual AppiumWebElement SearchContext { get; set; }

        /// <summary>
        /// Wait for typified control by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="AppiumWebElement"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            AppiumWebElement elementToWrap = GetNativeControl(locator);
            return Wrap<T>(elementToWrap, locator);
        }

        /// <summary>
        /// Wait for typified controls list by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="AppiumWebElement"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped controls list</returns>
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

        /// <summary>
        /// Get first child from current context which has specified control type ignoring implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="AppiumWebElement"/></typeparam>
        /// <returns>wrapped control instance</returns>
        protected override T GetFirstChildWrappedControl<T>()
        {
            var elementToWrap = GetNativeControlsList(new ByLocator(Using.WebXpath, "./*"))[0];
            return Wrap<T>(elementToWrap, null);
        }

        /// <summary>
        /// Get control instance from current context as <see cref="AppiumWebElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="AppiumWebElement"/> instance</returns>
        protected AppiumWebElement GetNativeControl(ByLocator locator) =>
            GetNativeControlFromContext(locator, SearchContext);

        /// <summary>
        /// Get control instance from parent context as <see cref="AppiumWebElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="AppiumWebElement"/> instance</returns>
        protected AppiumWebElement GetNativeControlFromParentContext(ByLocator locator) =>
            GetNativeControlFromContext(locator, ParentSearchContext.SearchContext);

        /// <summary>
        /// Wraps <see cref="AppiumWebElement"/> from current search context by specified locator.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="AppiumWebElement"/></typeparam>
        /// <param name="elementToWrap"><see cref="AppiumWebElement"/> instance to wrap</param>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected abstract T Wrap<T>(AppiumWebElement elementToWrap, ByLocator locator);

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
