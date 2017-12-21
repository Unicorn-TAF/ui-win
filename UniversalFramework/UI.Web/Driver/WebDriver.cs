using System;
using System.Text.RegularExpressions;
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
        private static Browser browser = Browser.CHROME;
        private static bool needInit = false;
        private static DriverOptions options = null;

        private static WebDriver instance = null;

        private WebDriver(bool maximize = true)
        {
            if (options == null)
            {
                Driver = GetInstance();
            }
            else
            {
                Driver = GetInstance(options);
            }

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
                    Logger.Instance.Debug($"{Browser} WebDriver initialized");
                }

                return instance;
            }
        }

        public static IWebDriver Driver { get; set; }

        public static Browser Browser
        {
            get
            {
                return browser;
            }

            set
            {
                browser = value;
            }
        }

        public string Url
        {
            get
            {
                return Driver.Url;
            }
        }

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

        public static void Init(Browser browser, DriverOptions driverPptions = null)
        {
            needInit = true;
            Browser = browser;
            options = driverPptions;
        }

        public void Get(string path)
        {
            Driver.Navigate().GoToUrl(path);
        }

        public object ExecuteJS(string script, params object[] parameters)
        {
            IJavaScriptExecutor js = Driver as IJavaScriptExecutor;
            return js.ExecuteScript(script, parameters);
        }

        public void Close()
        {
            if (instance != null)
            {
                Driver.Quit();
                instance = null;

                Browser = Browser.FIREFOX;
                options = null;
            }
        }

        private IWebDriver GetInstance()
        {
            switch (Browser)
            {
                case Browser.CHROME:
                    return new ChromeDriver();
                case Browser.IE:
                    return new InternetExplorerDriver();
                case Browser.FIREFOX:
                    return new FirefoxDriver();
                default:
                    return null;
            }
        }

        private IWebDriver GetInstance(DriverOptions options)
        {
            switch (Browser)
            {
                case Browser.CHROME:
                    return new ChromeDriver((ChromeOptions)options);
                case Browser.IE:
                    return new InternetExplorerDriver((InternetExplorerOptions)options);
                case Browser.FIREFOX:
                    return new FirefoxDriver((FirefoxOptions)options);
                default:
                    return null;
            }
        }

        ////public static string TransformXpath(string xpath)
        ////{
        ////    string textRegex = @"'(\w*\s*-*\(*\)*/*)+'";
        ////    Regex regex = new Regex(@"text\(\)\s*=\s*" + textRegex);

        ////    string textToTransform = regex.Match(xpath).Value;

        ////    if (string.IsNullOrEmpty(textToTransform))
        ////    {
        ////        return xpath;
        ////    }

        ////    string[] parts = xpath.Split(new string[] { textToTransform }, StringSplitOptions.None);

        ////    regex = new Regex(textRegex);
        ////    string replacementString = regex.Match(textToTransform).Value.ToLower();
        ////    textToTransform = regex.Replace(textToTransform, replacementString);

        ////    textToTransform = textToTransform.Replace("text()", "translate(normalize-space(text()), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')");

        ////    string newXpath = parts[0] + textToTransform + parts[1];

        ////    return newXpath;
        ////}
    }
}
