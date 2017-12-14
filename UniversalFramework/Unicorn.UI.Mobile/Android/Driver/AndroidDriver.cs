using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidDriver : AndroidSearchContext, IDriver
    {
        public static AppiumDriver<AndroidElement> Driver;
        private static DesiredCapabilities capabilities = null;
        private static Uri uri = null;
        private static bool needInit = false;

        private static AndroidDriver instance = null;

        private AndroidDriver()
        {
            Driver = new AndroidDriver<AndroidElement>(uri, capabilities);
            ImplicitlyWait = timeoutDefault;
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
                    Logger.Instance.Debug($"AndroidDriver initialized");
                }

                return instance;
            }
        }

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
