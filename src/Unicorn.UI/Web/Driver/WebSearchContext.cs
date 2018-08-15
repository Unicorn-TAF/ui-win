using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    public abstract class WebSearchContext : UISearchContext<WebSearchContext>
    {
        protected virtual OpenQA.Selenium.ISearchContext SearchContext { get; set; }

        protected override Type ControlsBaseType => typeof(WebControl);

        #region "Helpers"

        protected override T WaitForWrappedControl<T>(ByLocator locator)
        {
            IWebElement elementToWrap = GetNativeControl(locator);
            return this.Wrap<T>(elementToWrap, locator);
        }

        protected override IList<T> GetWrappedControlsList<T>(ByLocator locator)
        {
            var elementsToWrap = GetNativeControlsList(locator);
            List<T> controlsList = new List<T>();

            foreach (var elementToWrap in elementsToWrap)
            {
                controlsList.Add(this.Wrap<T>(elementToWrap, null));
            }

            return controlsList;
        }

        protected override T GetFirstChildWrappedControl<T>()
        {
            var elementToWrap = GetNativeControlsList(new ByLocator(Using.Web_Xpath, "./*"))[0];
            return this.Wrap<T>(elementToWrap, null);
        }

        protected IWebElement GetNativeControl(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, this.SearchContext);
        }

        protected IWebElement GetNativeControlFromParentContext(ByLocator locator)
        {
            return GetNativeControlFromContext(locator, this.ParentSearchContext.SearchContext);
        }

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            WebDriver.Instance.ImplicitlyWait = timeout;
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
