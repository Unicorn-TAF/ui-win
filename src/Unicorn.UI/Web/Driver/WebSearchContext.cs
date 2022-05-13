using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Describes search context for web controls. Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public abstract class WebSearchContext : BaseSearchContext<WebSearchContext>
    {
        /// <summary>
        /// Gets or sets Current search context as <see cref="OpenQA.Selenium.ISearchContext"/>
        /// </summary>
        protected virtual OpenQA.Selenium.ISearchContext SearchContext { get; set; }

        /// <summary>
        /// Gets or sets base <see cref="Type"/> for all desktop controls (by default is <see cref="WebControl"/>)
        /// </summary>
        protected override Type ControlsBaseType => typeof(WebControl);

        #region "Helpers"

        /// <summary>
        /// Wait for typified control by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            IWebElement elementToWrap = GetNativeControl(locator);
            return Wrap<T>(elementToWrap, locator);
        }

        /// <summary>
        /// Wait for typified controls list by specified locator during implicitly wait timeout.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
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
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="WebControl"/></typeparam>
        /// <returns>wrapped control instance</returns>
        protected override T GetFirstChildWrappedControl<T>()
        {
            var elementToWrap = GetNativeControlsList(new ByLocator(Using.WebXpath, "./*"))[0];
            return Wrap<T>(elementToWrap, null);
        }

        /// <summary>
        /// Get control instance from current context as <see cref="IWebElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="IWebElement"/> instance</returns>
        protected IWebElement GetNativeControl(ByLocator locator) =>
            GetNativeControlFromContext(locator, SearchContext);

        /// <summary>
        /// Get control instance from parent context as <see cref="IWebElement"/>.
        /// </summary>
        /// <param name="locator">locator to search by</param>
        /// <returns><see cref="IWebElement"/> instance</returns>
        protected IWebElement GetNativeControlFromParentContext(ByLocator locator) =>
            GetNativeControlFromContext(locator, ParentSearchContext.SearchContext);

        /// <summary>
        /// Set current implicitly wait timeout value.
        /// </summary>
        /// <param name="timeout">new implicit timeout value</param>
        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            IWebDriver driver = SearchContext is IWebDriver ?
                (IWebDriver)SearchContext :
                ((OpenQA.Selenium.Internal.IWrapsDriver)SearchContext).WrappedDriver;

            driver.Manage().Timeouts().ImplicitWait = timeout;
        }

        private IWebElement GetNativeControlFromContext(ByLocator locator, OpenQA.Selenium.ISearchContext context)
        {
            By by = GetNativeLocator(locator);
            try
            {
                IWebElement nativeControl = context.FindElement(by);
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

        private T Wrap<T>(IWebElement elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((WebControl)(object)wrapper).Instance = elementToWrap;
            ((WebControl)(object)wrapper).ParentSearchContext = this;
            ((WebControl)(object)wrapper).Locator = locator;
            return wrapper;
        }

        #endregion
    }
}
