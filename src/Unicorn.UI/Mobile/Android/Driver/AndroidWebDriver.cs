using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Remote;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidWebDriver : WebDriver
    {
        public AndroidWebDriver(string hubUrl, DesiredCapabilities desiredCapabilities)
        {
            SeleniumDriver = new AndroidDriver<IWebElement>(new Uri(hubUrl), desiredCapabilities);
            ImplicitlyWait = TimeoutDefault;
        }

        public AndroidWebDriver(string hubUrl, string deviceName, string browserName, string platformVersion)
        {
            var capabilities = new DesiredCapabilities();
            capabilities.SetCapability("deviceName", deviceName);
            capabilities.SetCapability("browserName", browserName);
            capabilities.SetCapability("platformVersion", platformVersion);
            capabilities.SetCapability("platformName", "Android");

            SeleniumDriver = new AndroidDriver<IWebElement>(new Uri(hubUrl), capabilities);
            ImplicitlyWait = TimeoutDefault;
        }
    }
}
