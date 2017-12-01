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
        private static DesiredCapabilities _Capabilities = null;
        private static Uri _Uri = null; 
        static bool _needInit = false;

        private static AndroidDriver _instance = null;
        public static AndroidDriver Instance
        {
            get
            {
                if (_instance == null || _needInit)
                {
                    _instance = new AndroidDriver();
                    _instance.SearchContext = Driver.FindElementByClassName("android.widget.FrameLayout");
                    _needInit = false;
                    Logger.Instance.Debug($"AndroidDriver initialized");
                }

                return _instance;
            }
        }

        public static void Init(string url, Dictionary<string, string> capabilitiesList = null)
        {
            _needInit = true;
            _Uri = new Uri(url);

            _Capabilities = null;
            if (capabilitiesList != null)
            {
                _Capabilities = new DesiredCapabilities();
                foreach (string key in capabilitiesList.Keys)
                    _Capabilities.SetCapability(key, capabilitiesList[key]);
            }
        }

        private AndroidDriver()
        {
            Driver = new AndroidDriver<AndroidElement>(_Uri, _Capabilities);
            ImplicitlyWait = _timeoutDefault;
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
