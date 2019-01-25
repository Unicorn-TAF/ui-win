using System;
using OpenQA.Selenium;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;

namespace Unicorn.UI.Web.Driver
{
    public abstract class WebDriver : WebSearchContext, IDriver
    {
        private static WebDriver instance = null;

        public static WebDriver Instance
        {
            get
            {
                return instance;
            }

            set
            {
                Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"{value.Browser} {value.GetType()} driver initialized");
                instance = value;
                instance.SearchContext = Driver;
            }
        }

        public static IWebDriver Driver { get; set; }

        public BrowserType Browser { get; protected set; }

        public string Url => Driver.Url;

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return Driver.Manage().Timeouts().ImplicitWait;
            }

            set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
            }
        }

        public static void Close()
        {
            Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, "Close driver");

            if (Instance != null)
            {
                Driver.Quit();
                Instance = null;
            }
        }

        public void Get(string url)
        {
            Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"Navigate to {url} page");
            Driver.Navigate().GoToUrl(url);
        }

        public object ExecuteJS(string script, params object[] parameters)
        {
            Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"Executing JS: {script}");
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            return js.ExecuteScript(script, parameters);
        }

        public void ScrollTo(WebControl control)
        {
            this.ExecuteJS("window.scrollTo({0}, {1});", control.Location.X, control.Location.Y);
        }
    }
}
