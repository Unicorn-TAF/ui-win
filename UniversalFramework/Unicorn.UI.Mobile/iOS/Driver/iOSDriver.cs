using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.IOS.Driver
{
    public class IOSDriver : IOSSearchContext, IDriver
    {
        private static DesiredCapabilities capabilities = null;
        private static Uri uri = null;
        private static bool needInit = false;
        private static IOSDriver instance = null;

        private IOSDriver()
        {
            Driver = new OpenQA.Selenium.Appium.iOS.IOSDriver<IOSElement>(uri, capabilities, TimeSpan.FromSeconds(120));
            this.ImplicitlyWait = TimeoutDefault;
        }

        public static IOSDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new IOSDriver();
                    instance.SearchContext = Driver.FindElementByXPath(".//*");
                    needInit = false;
                    Logger.Instance.Log(LogLevel.Info, instance.SearchContext.TagName);
                    Logger.Instance.Log(LogLevel.Debug, $"iOSDriver initialized");
                }

                return instance;
            }
        }

        public static AppiumDriver<IOSElement> Driver { get; set; }

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return ImplicitlyWaitTimeout;
            }

            set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
                ImplicitlyWaitTimeout = value;
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

        public void Get(string path)
        {
            Driver.Navigate().GoToUrl(path);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
