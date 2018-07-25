using System.Collections.Generic;
using Unicorn.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public abstract class AndroidApplication
    {
        protected AndroidApplication(string appPackage, string appActivity, string platformVersion, string deviceName, string hubUrl)
        {
            this.DeviceName = deviceName;
            this.PlatformVersion = platformVersion;
            this.AppPackage = appPackage;
            this.AppActivity = appActivity;
            this.HubUrl = hubUrl;

            var capabilities = new Dictionary<string, string>();
            capabilities.Add("deviceName", this.DeviceName);
            capabilities.Add("platformVersion", this.PlatformVersion);
            capabilities.Add("appPackage", this.AppPackage);
            capabilities.Add("appActivity", $"{this.AppPackage}.{this.AppActivity}");
            capabilities.Add("platformName", this.PlatformName);


            AndroidDriver.Init(hubUrl, capabilities);
        }

        public string AppPackage { get; protected set; }

        public string AppActivity { get; protected set; }

        public string PlatformName { get; protected set; } = "Android";

        public string PlatformVersion { get; protected set; }

        public string DeviceName { get; protected set; }

        public string HubUrl { get; protected set; }

        public void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {this.AppActivity} application");
            AndroidDriver.Instance.GetType();
        }
    }
}
