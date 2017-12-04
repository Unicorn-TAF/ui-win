using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.iOS.Driver
{
    public class iOSDriver : iOSSearchContext, IDriver
    {

        public static AppiumDriver<IOSElement> Driver;
        private static DesiredCapabilities _Capabilities = null;
        private static Uri _Uri = null; 
        static bool _needInit = false;

        private static iOSDriver _instance = null;
        public static iOSDriver Instance
        {
            get
            {
                if (_instance == null || _needInit)
                {
                    _instance = new iOSDriver();
                    _instance.SearchContext = Driver.FindElementByXPath(".//*");
                    _needInit = false;
                    Logger.Instance.Info(_instance.SearchContext.TagName);
                    Logger.Instance.Debug($"iOSDriver initialized");
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

        private iOSDriver()
        {
            Driver = new IOSDriver<IOSElement>(_Uri, _Capabilities, TimeSpan.FromSeconds(120));
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
