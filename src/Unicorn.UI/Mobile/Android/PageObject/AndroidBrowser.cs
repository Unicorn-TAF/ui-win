using System.Collections.Generic;
using Unicorn.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public class AndroidBrowser
    {
        protected AndroidBrowser(string browserName, string platformVersion, string deviceName, string hubUrl)
        {
            this.DeviceName = deviceName;
            this.PlatformVersion = platformVersion;
            this.BrowserName = browserName;
            this.HubUrl = hubUrl;

            var capabilities = new Dictionary<string, string>();
            capabilities.Add("deviceName", this.DeviceName);
            capabilities.Add("platformVersion", this.PlatformVersion);
            capabilities.Add("browserName", this.BrowserName);
            capabilities.Add("platformName", this.PlatformName);

            AndroidDriver.Init(hubUrl, capabilities);
        }

        public string BrowserName { get; protected set; }

        public string AppActivity { get; protected set; }

        public string PlatformName { get; protected set; } = "Android";

        public string PlatformVersion { get; protected set; }

        public string DeviceName { get; protected set; }

        public string HubUrl { get; protected set; }

        public void Open(string url)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {url} website");
            AndroidDriver.Instance.Get(url);
        }
    }
}
