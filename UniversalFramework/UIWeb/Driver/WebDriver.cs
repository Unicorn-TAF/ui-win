using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Text.RegularExpressions;
using UICore.Driver;

namespace UIWeb.Driver
{
    public class WebDriver : WebSearchContext, IDriver
    {
        private static IWebDriver _driver = null;

        private static WebDriver _instance;
        public static WebDriver Instance
        {
            get
            {
                if (_driver == null)
                {
                    _instance = new WebDriver();
                    _driver = new FirefoxDriver();
                    _driver.Manage().Window.Maximize();
                    _instance.SearchContext = _driver;
                }

                return _instance;
            }
        }

        public void SetImplicitlyWait(TimeSpan time)
        {
            _driver.Manage().Timeouts().ImplicitlyWait(time);
        }

        public void Get(string path)
        {
            _driver.Navigate().GoToUrl(path);
        }

        public void Close()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
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
