using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidWebDriver : WebSearchContext, IDriver
    {
        private static DesiredCapabilities capabilities = null;
        private static Uri uri = null;
        private static bool needInit = false;

        private static AndroidWebDriver instance = null;

        private AndroidWebDriver()
        {
            Driver = new AndroidDriver<OpenQA.Selenium.IWebElement>(uri, capabilities);
            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public static AndroidWebDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new AndroidWebDriver();
                    instance.SearchContext = Driver;
                    needInit = false;
                    Logger.Instance.Log(LogLevel.Debug, $"MobileWebDriver initialized");
                }

                return instance;
            }
        }

        public static AppiumDriver<OpenQA.Selenium.IWebElement> Driver { get; set; }

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

        public static void Init(string url, Dictionary<string, string> capabilitiesList = null)
        {
            needInit = true;
            uri = new Uri(url);

            capabilities = null;

            if (capabilitiesList != null)
            {
                capabilities = new DesiredCapabilities();
                foreach (string key in capabilitiesList.Keys)
                {
                    capabilities.SetCapability(key, capabilitiesList[key]);
                }
            }
        }

        public static void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, "Close driver");

            if (instance != null)
            {
                Driver.Quit();
                instance = null;
            }
        }

        public void Get(string path)
        {
            Driver.Navigate().GoToUrl(path);
            instance.SearchContext = Driver;
        }
    }
}
