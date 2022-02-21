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
        private static DesiredCapabilities _capabilities = null;
        private static Uri _uri = null;
        private static bool _needInit = false;

        private static AndroidAppDriver _instance = null;

        private AndroidAppDriver()
        {
            Driver = new AndroidDriver<AndroidElement>(_uri, null);  // TODO: to fix second parameter
            ImplicitlyWait = TimeoutDefault;
        }

        public static AndroidAppDriver Instance
        {
            get
            {
                if (_instance == null || _needInit)
                {
                    _instance = new AndroidAppDriver();
                    Driver.FindElementByClassName("android.widget.FrameLayout");
                    _needInit = false;
                    Logger.Instance.Log(LogLevel.Debug, $"AndroidDriver initialized");
                }

                return _instance;
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
            _needInit = true;
            _uri = new Uri(url);

            _capabilities = null;

            if (capabilitiesList != null)
            {
                _capabilities = new DesiredCapabilities();
                foreach (string key in capabilitiesList.Keys)
                {
                    _capabilities.SetCapability(key, capabilitiesList[key]);
                }
            }
        }

        public static void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, "Close driver");

            if (_instance != null)
            {
                Driver.Quit();
                _instance = null;
            }
        }
    }
}
