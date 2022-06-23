using OpenQA.Selenium.Appium;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Mobile.Android.Driver;

namespace Unicorn.UI.Mobile.Android.PageObject
{
    /// <summary>
    /// Describes basic android application with driver attached to it.
    /// </summary>
    public abstract class AndroidApplication
    {
        private readonly AppiumOptions _options;
        private readonly string _appActivity;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidApplication"/> class 
        /// with specified application and appium parameters.
        /// </summary>
        /// <param name="appPackage">appPackage value</param>
        /// <param name="appActivity">appActivity value</param>
        /// <param name="platformVersion">android version</param>
        /// <param name="deviceName">name of device</param>
        /// <param name="hubUrl">url to appium hub</param>
        protected AndroidApplication(string appPackage, string appActivity, string platformVersion, string deviceName, string hubUrl)
        {
            HubUrl = hubUrl;
            _appActivity = $"{appPackage}.{appActivity}";

            _options = new AppiumOptions();

            _options.AddAdditionalCapability("deviceName", deviceName);
            _options.AddAdditionalCapability("platformVersion", platformVersion);
            _options.AddAdditionalCapability("appPackage", appPackage);
            _options.AddAdditionalCapability("appActivity", _appActivity);
            _options.AddAdditionalCapability("platformName", "Android");
        }

        /// <summary>
        /// Gets <see cref="AndroidAppDriver"/> instance used by the application.
        /// </summary>
        public AndroidAppDriver Driver { get; private set; }

        /// <summary>
        /// Gets appium hub url.
        /// </summary>
        public string HubUrl { get; }

        /// <summary>
        /// Starts the application and initializes associated driver.
        /// </summary>
        public virtual void Open()
        {
            Logger.Instance.Log(LogLevel.Debug, $"Open {_appActivity} application");
            Driver = new AndroidAppDriver(HubUrl, _options);
        }
    }
}
