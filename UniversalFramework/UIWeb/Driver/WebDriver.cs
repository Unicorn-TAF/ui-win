using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using System;
using System.Text.RegularExpressions;
using Unicorn.UICore.Driver;

namespace Unicorn.UIWeb.Driver
{
    public class WebDriver : WebSearchContext, IDriver
    {
        public Browser Browser;
        private IWebDriver Driver;

        public WebDriver(Browser browser, bool maximize = true)
        {
            Driver = GetInstance(browser);
            SearchContext = Driver;

            if (maximize)
                Driver.Manage().Window.Maximize();
        }

        public WebDriver(Browser browser, DriverOptions options, bool maximize = true)
        {
            Driver = GetInstance(browser, options);
            SearchContext = Driver;

            if (maximize)
                Driver.Manage().Window.Maximize();
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
            Driver.Quit();
        }


        private IWebDriver GetInstance(Browser browser)
        {
            Browser = browser;

            switch (browser)
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

        private IWebDriver GetInstance(Browser browser, DriverOptions options)
        {
            Browser = browser;

            switch (browser)
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
