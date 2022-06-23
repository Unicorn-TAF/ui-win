using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    /// <summary>
    /// Represents driver for browser on android mobile device and allows to perform search of elements in web pages.
    /// </summary>
    public class AndroidWebDriver : WebDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidWebDriver"/> class with specified url and options.
        /// </summary>
        /// <param name="hubUrl">server url</param>
        /// <param name="options"><see cref="AppiumOptions"/> instance</param>
        public AndroidWebDriver(string hubUrl, AppiumOptions options)
        {
            SeleniumDriver = new AndroidDriver<IWebElement>(new Uri(hubUrl), options);
            ImplicitlyWait = TimeoutDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidWebDriver"/> class with specified and device parameters.
        /// </summary>
        /// <param name="hubUrl">server url</param>
        /// <param name="deviceName">device name</param>
        /// <param name="browserName">browser name</param>
        /// <param name="platformVersion">android version</param>
        public AndroidWebDriver(string hubUrl, string deviceName, string browserName, string platformVersion)
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability("deviceName", deviceName);
            options.AddAdditionalCapability("browserName", browserName);
            options.AddAdditionalCapability("platformVersion", platformVersion);
            options.AddAdditionalCapability("platformName", "Android");

            SeleniumDriver = new AndroidDriver<IWebElement>(new Uri(hubUrl), options);
            ImplicitlyWait = TimeoutDefault;
        }
    }
}
