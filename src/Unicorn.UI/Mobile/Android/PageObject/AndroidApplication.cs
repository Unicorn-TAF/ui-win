using System.Collections.Generic;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    public abstract class AndroidApplication
    {
        private readonly Dictionary<string, string> _options;
        private readonly string _appActivity;

        protected AndroidApplication(string appPackage, string appActivity, string platformVersion, string deviceName, string hubUrl)
        {
            HubUrl = hubUrl;
            _appActivity = $"{appPackage}.{appActivity}";

            _options = new Dictionary<string, string>();
            _options.Add("deviceName", deviceName);
            _options.Add("platformVersion", platformVersion);
            _options.Add("appPackage", appPackage);
            _options.Add("appActivity", _appActivity);
            _options.Add("platformName", "Android");
        }

        /// <summary>
        /// Gets AndroidAppDriver instance used by the website.
        /// </summary>
        public AndroidAppDriver Driver { get; private set; }

        public string HubUrl { get; }

        public virtual void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {_appActivity} application");
            Driver = new AndroidAppDriver(HubUrl, _options);
        }
    }
}
