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
        private static WebDriver _instance = null;

        private TimeSpan currentImplicitlyWait;

        /// <summary>
        /// Gets or sets instance of Web driver.
        /// Initialized with default implicitly wait timeout.
        /// </summary>
        public static WebDriver Instance
        {
            get
            {
                return _instance;
            }

            set
            {
                if (value != null)
                {
                    Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, $"{value.Browser} {value.GetType()} driver initialized");
                    _instance = value;
                    _instance.SearchContext = _instance.SeleniumDriver;
                }
                else
                {
                    _instance = null;
                }
            }
        }

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
            get
            {
                return currentImplicitlyWait;
            }

            set
            {
                currentImplicitlyWait = value;
                SeleniumDriver.Manage().Timeouts().ImplicitWait = value;
            }
        }

        /// <summary>
        /// Close web driver instance and associated browser.
        /// </summary>
        public static void Close()
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, "Close driver");

            if (Instance != null)
            {
                Instance.SeleniumDriver.Quit();
                Instance = null;
            }
        }

        /// <summary>
        /// Navigates to specified URL.
        /// </summary>
        /// <param name="url">url to navigate</param>
        public void Get(string url)
        {
            Logger.Instance.Log(Taf.Core.Logging.LogLevel.Debug, $"Navigate to {url} page");
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
        public void ScrollTo(WebControl control) =>
            ExecuteJS("window.scrollTo({0}, {1});", control.Location.X, control.Location.Y);
    }
}
