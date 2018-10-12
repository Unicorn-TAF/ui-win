using System.Collections.Generic;
using Unicorn.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;
using Unicorn.UI.Web.PageObject;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public abstract class AndroidWebSite : WebSite
    {
        protected override OpenQA.Selenium.ISearchContext Context => AndroidWebDriver.Driver;

        protected AndroidWebSite(string browserName, string platformVersion, string deviceName, string hubUrl, string baseUrl) : base(baseUrl)
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

            AndroidWebDriver.Init(hubUrl, capabilities);
        }

        public string BrowserName { get; protected set; }

        public string AppActivity { get; protected set; }

        public string PlatformName { get; protected set; } = "Android";

        public string PlatformVersion { get; protected set; }

        public string DeviceName { get; protected set; }

        public string HubUrl { get; protected set; }

        public override T NavigateTo<T>()
        {
            var page = GetPage<T>();
            Logger.Instance.Log(LogLevel.Debug, $"Navigate to {page.Url} page");
            AndroidWebDriver.Instance.Get(this.BaseUrl + page.Url);
            return page;
        }

        public override void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {this.BaseUrl} site");
            AndroidWebDriver.Instance.Get(this.BaseUrl);
        }
    }
}
