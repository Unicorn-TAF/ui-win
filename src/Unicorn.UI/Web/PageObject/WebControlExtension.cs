using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.PageObject
{
    /// <summary>
    /// Extension for <see cref="WebControl"/>
    /// </summary>
    public static class WebControlExtension
    {
        /// <summary>
        /// Check if control exists int page object immediately ignoring implicitly wait.
        /// </summary>
        /// <param name="control">Control instance</param>
        /// <returns>true - if control exists; otherwise - false</returns>
        public static bool Exists(this WebControl control)
        {
            IWebDriver driver = ((IWrapsDriver)control.Instance).WrappedDriver;
            TimeSpan originalTimeout = driver.Manage().Timeouts().ImplicitWait;
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            try
            {
                return control.Instance.GetType() != null;
            }
            catch (ControlNotFoundException)
            {
                return false;
            }
            finally
            {
                driver.Manage().Timeouts().ImplicitWait = originalTimeout;
            }
        }
    }
}
