using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.Controls;
using Unicorn.UI.Mobile.Base.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    /// <summary>
    /// Describes search context for android controls. 
    /// Contains variety of methods to search and wait for controls of specified type from current context.
    /// </summary>
    public class AndroidSearchContext : MobileSearchContext
    {
        /// <summary>
        /// Gets or sets base <see cref="Type"/> for all andriod controls (by default is <see cref="AndroidControl"/>)
        /// </summary>
        protected override Type ControlsBaseType => typeof(AndroidControl);

        /// <summary>
        /// Set current implicitly wait timeout value.
        /// </summary>
        /// <param name="timeout">new implicit timeout value</param>
        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            IWebDriver driver = ((OpenQA.Selenium.Internal.IWrapsDriver)SearchContext).WrappedDriver;

            driver.Manage().Timeouts().ImplicitWait = timeout;
        }

        /// <summary>
        /// Wraps <see cref="AppiumWebElement"/> from current search context by specified locator 
        /// with <see cref="AndroidControl"/>.
        /// </summary>
        /// <typeparam name="T">any <see cref="Type"/> inherited from <see cref="AppiumWebElement"/></typeparam>
        /// <param name="elementToWrap"><see cref="AppiumWebElement"/> instance to wrap</param>
        /// <param name="locator">locator to search by</param>
        /// <returns>wrapped control instance</returns>
        protected override T Wrap<T>(AppiumWebElement elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((AndroidControl)(object)wrapper).Instance = elementToWrap;
            ((AndroidControl)(object)wrapper).ParentSearchContext = this;
            ((AndroidControl)(object)wrapper).Locator = locator;
            return wrapper;
        }
    }
}
