using System;
using OpenQA.Selenium;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Represents Driver for Web UI and allows to perform search of elements in web pages.
    /// </summary>
    public abstract class WebDriver : WebSearchContext, IDriver
    {
        /// <summary>
        /// Gets or sets underlying <see cref="IWebDriver"/> instance.
        /// </summary>
        public IWebDriver SeleniumDriver { get; set; }

        /// <summary>
        /// Gets or sets browser type.
        /// </summary>
        public BrowserType Browser { get; protected set; }

        /// <summary>
        /// Gets current URL.
        /// </summary>
        public string Url => SeleniumDriver.Url;

        /// <summary>
        /// Gets or sets implicit timeout of waiting for specified element to be existed in elements tree.
        /// </summary>
        public TimeSpan ImplicitlyWait
        {
            get => SeleniumDriver.Manage().Timeouts().ImplicitWait;

            set => SeleniumDriver.Manage().Timeouts().ImplicitWait = value;
        }

        /// <summary>
        /// Close web driver instance and associated browser.
        /// </summary>
        public void Close()
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, "Close driver");
            SeleniumDriver.Quit();
        }

        /// <summary>
        /// Navigates to specified URL.
        /// </summary>
        /// <param name="url">url to navigate</param>
        public void Get(string url)
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, $"Navigate to {url}");
            SeleniumDriver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Executes JavaScript.
        /// </summary>
        /// <param name="script">script string</param>
        /// <param name="parameters">array of script parameters if any</param>
        /// <returns>result of script execution as <see cref="object"/></returns>
        public object ExecuteJS(string script, params object[] parameters)
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, $"Executing JS: {script}");
            IJavaScriptExecutor js = SeleniumDriver as IJavaScriptExecutor;
            return js.ExecuteScript(script, parameters);
        }

        /// <summary>
        /// Scroll view to specified control position.
        /// </summary>
        /// <param name="control">control instance</param>
        public void ScrollTo(WebControl control)
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, "Scroll to " + control);

            IJavaScriptExecutor js = SeleniumDriver as IJavaScriptExecutor;
            js.ExecuteScript(
                "arguments[0].scrollIntoView(true); window.scrollTo(0, arguments[0].getBoundingClientRect().top + window.pageYOffset - (window.innerHeight / 2));", 
                control.Instance);
        }
    }
}
