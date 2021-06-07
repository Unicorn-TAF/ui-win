using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Ios.Driver
{
    public class IosDriver : IosSearchContext, IDriver
    {
        private static DesiredCapabilities _capabilities = null;
        private static Uri _uri = null;
        private static bool _needInit = false;
        private static IosDriver _instance = null;

        private IosDriver()
        {
            Driver = new IOSDriver<IOSElement>(_uri, _capabilities, TimeSpan.FromSeconds(120));
            ImplicitlyWait = TimeoutDefault;
        }

        public static IosDriver Instance
        {
            get
            {
                if (_instance == null || _needInit)
                {
                    _instance = new IosDriver();
                    _instance.SearchContext = Driver.FindElementByXPath(".//*");
                    _needInit = false;
                    Logger.Instance.Log(LogLevel.Info, _instance.SearchContext.TagName);
                    Logger.Instance.Log(LogLevel.Debug, $"iOSDriver initialized");
                }

                return _instance;
            }
        }

        public static AppiumDriver<IOSElement> Driver { get; set; }

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
