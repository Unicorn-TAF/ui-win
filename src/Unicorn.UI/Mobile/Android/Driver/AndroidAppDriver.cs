using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidAppDriver : AndroidSearchContext, IDriver
    {
        private static DesiredCapabilities capabilities = null;
        private static Uri uri = null;
        private static bool needInit = false;

        private static AndroidAppDriver instance = null;

        private AndroidAppDriver()
        {
            Driver = new AndroidDriver<AndroidElement>(uri, capabilities);
            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public static AndroidAppDriver Instance
        {
            get
            {
                if (instance == null || needInit)
                {
                    instance = new AndroidAppDriver();
                    Driver.FindElementByClassName("android.widget.FrameLayout");
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
    }
}
