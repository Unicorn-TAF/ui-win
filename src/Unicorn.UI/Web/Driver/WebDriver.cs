using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Web.Driver
{
    public class WebDriver : WebSearchContext, IDriver
    {
        private static bool needInit = false;
        private static DriverOptions options = null;

        private static WebDriver instance = null;

        private WebDriver(bool maximize = true)
        {
            Driver = options == null ? GetInstance() : GetInstance(options);

            if (maximize)
            {
                Driver.Manage().Window.Maximize();
            }

            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public static WebDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new WebDriver();
                    instance.SearchContext = Driver;
                    needInit = false;
                    Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"{Browser} WebDriver initialized");
                }

                return instance;
            }
        }

        public static IWebDriver Driver { get; set; }

        public static BrowserType Browser { get; set; } = BrowserType.Chrome;

        public string Url => Driver.Url;

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return WebSearchContext.ImplicitlyWaitTimeout;
            }

            set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
                WebSearchContext.ImplicitlyWaitTimeout = value;
            }
        }

        public static void Init(BrowserType browser, DriverOptions driverOptions = null)
        {
            needInit = true;
            Browser = browser;
            options = driverOptions;
        }

        public static void Close()
        {
            Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, "Close driver");

            if (instance != null)
            {
                Driver.Quit();
                instance = null;

                options = null;
            }
        }

        public void Get(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public object ExecuteJS(string script, params object[] parameters)
        {
            Logger.Instance.Log(Unicorn.Core.Logging.LogLevel.Debug, $"Executing JS: {script}");
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            return js.ExecuteScript(script, parameters);
        }

        private IWebDriver GetInstance()
        {
            switch (Browser)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver();
                case BrowserType.IE:
                    return new InternetExplorerDriver();
                case BrowserType.Firefox:
                    return new FirefoxDriver();
                default:
                    return null;
            }
        }

        private IWebDriver GetInstance(DriverOptions options)
        {
            switch (Browser)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver((ChromeOptions)options);
                case BrowserType.IE:
                    return new InternetExplorerDriver((InternetExplorerOptions)options);
                case BrowserType.Firefox:
                    return new FirefoxDriver((FirefoxOptions)options);
                default:
                    return null;
            }
        }
    }
}
