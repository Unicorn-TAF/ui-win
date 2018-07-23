using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Base.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidDriver : AndroidSearchContext, IDriver
    {
        private static DesiredCapabilities capabilities = null;
        private static Uri uri = null;
        private static bool needInit = false;

        private static AndroidDriver instance = null;

        private AndroidDriver()
        {
            Driver = new AndroidDriver<AndroidElement>(uri, capabilities);
            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public static AndroidDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new AndroidDriver();
                    instance.SearchContext = Driver.FindElementByClassName("android.widget.FrameLayout");
                    needInit = false;
                    Logger.Instance.Log(LogLevel.Debug, $"AndroidDriver initialized");
                }

                return instance;
            }
        }

        public static AppiumDriver<AndroidElement> Driver { get; set; }

        public TimeSpan ImplicitlyWait
        {
            get
            {
                return MobileSearchContext.ImplicitlyWaitTimeout;
            }

            set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
                MobileSearchContext.ImplicitlyWaitTimeout = value;
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
