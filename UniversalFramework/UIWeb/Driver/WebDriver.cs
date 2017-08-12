using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Text.RegularExpressions;
using Unicorn.Core.Logging;
using Unicorn.UICore.Driver;

namespace Unicorn.UIWeb.Driver
{
    public class WebDriver : WebSearchContext, IDriver
    {
        public static Browser Browser = Browser.CHROME;
        private IWebDriver Driver;

        static bool _needInit = false;
        static DriverOptions _options = null;

        private static WebDriver _instance = null;
        public static WebDriver Instance
        {
            get
            {
                if (_instance == null || _needInit)
                {
                    _instance = new WebDriver();
                    _needInit = false;
                    Logger.Instance.Debug($"{Browser} WebDriver initialized");
                }

                return _instance;
            }
        }


        public static void Init(Browser browser, DriverOptions options = null)
        {
            _needInit = true;
            Browser = browser;
            _options = options;
        }


        private WebDriver(bool maximize = true)
        {
            if (_options == null)
                Driver = GetInstance();
            else
                Driver = GetInstance(_options);

            SearchContext = Driver;

            if (maximize)
                Driver.Manage().Window.Maximize();

            SetImplicitlyWait(_timeoutDefault);
        }


        public string Url
        {
            get
            {
                return Driver.Url;
            }
        }


        public void SetImplicitlyWait(TimeSpan time)
        {
            Driver.Manage().Timeouts().ImplicitlyWait(time);
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
            if (_instance != null)
            {
                Driver.Quit();
                _instance = null;

                Browser = Browser.FIREFOX;
                _options = null;
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



        public static string TransformXpath(string xpath)
        {
            string textRegex = @"'(\w*\s*-*\(*\)*/*)+'";
            Regex regex = new Regex(@"text\(\)\s*=\s*" + textRegex);

            string textToTransform = regex.Match(xpath).Value;

            if (string.IsNullOrEmpty(textToTransform))
                return xpath;

            string[] parts = xpath.Split(new string[] { textToTransform }, StringSplitOptions.None);

            regex = new Regex(textRegex);
            string replacementString = regex.Match(textToTransform).Value.ToLower();
            textToTransform = regex.Replace(textToTransform, replacementString);

            textToTransform = textToTransform.Replace("text()", "translate(normalize-space(text()), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')");

            string newXpath = parts[0] + textToTransform + parts[1];

            return newXpath;
        }
    }
}
