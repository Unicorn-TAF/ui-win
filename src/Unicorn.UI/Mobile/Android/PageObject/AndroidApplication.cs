using System.Collections.Generic;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public abstract class AndroidApplication
    {
        protected AndroidApplication(string appPackage, string appActivity, string platformVersion, string deviceName, string hubUrl)
        {
            DeviceName = deviceName;
            PlatformVersion = platformVersion;
            AppPackage = appPackage;
            AppActivity = appActivity;
            HubUrl = hubUrl;

            var capabilities = new Dictionary<string, string>();
            capabilities.Add("deviceName", DeviceName);
            capabilities.Add("platformVersion", PlatformVersion);
            capabilities.Add("appPackage", AppPackage);
            capabilities.Add("appActivity", $"{AppPackage}.{AppActivity}");
            capabilities.Add("platformName", PlatformName);

            AndroidAppDriver.Init(hubUrl, capabilities);
        }

        public string AppPackage { get; protected set; }

        public string AppActivity { get; protected set; }

        public string PlatformName { get; protected set; } = "Android";

        public string PlatformVersion { get; protected set; }

        public string DeviceName { get; protected set; }

        public string HubUrl { get; protected set; }

        public void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {AppActivity} application");
            AndroidAppDriver.Instance.ToString(); // need to call for initialization
        }
    }
}
