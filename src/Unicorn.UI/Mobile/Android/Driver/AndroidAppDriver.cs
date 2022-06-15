using System;
using System.Collections.Generic;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using Unicorn.Taf.Core.Logging;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    /// <summary>
    /// Represents driver for android applications and allows to perform search of elements.
    /// </summary>
    public class AndroidAppDriver : AndroidSearchContext, IDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAppDriver"/> class with specified 
        /// remote address, and driver options.
        /// </summary>
        /// <param name="remoteAddress">remote address</param>
        /// <param name="options"><see cref="AppiumOptions"/></param>
        public AndroidAppDriver(Uri remoteAddress, AppiumOptions options)
        {
            Driver = new AndroidDriver<AndroidElement>(remoteAddress, options);
            ImplicitlyWait = TimeoutDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAppDriver"/> class with specified 
        /// remote address, and driver options.
        /// </summary>
        /// <param name="remoteAddress">remote address</param>
        /// <param name="options"><see cref="AppiumOptions"/></param>
        public AndroidAppDriver(string remoteAddress, AppiumOptions options)
        {
            Driver = new AndroidDriver<AndroidElement>(new Uri(remoteAddress), options);
            ImplicitlyWait = TimeoutDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidAppDriver"/> class with specified 
        /// remote address, and driver options.
        /// </summary>
        /// <param name="remoteAddress">remote address</param>
        /// <param name="optionsList">dictionary of options</param>
        public AndroidAppDriver(string remoteAddress, Dictionary<string, string> optionsList)
        {
            AppiumOptions options = new AppiumOptions();

            foreach (string key in optionsList.Keys)
            {
                options.AddAdditionalCapability(key, optionsList[key]);
            }

            Driver = new AndroidDriver<AndroidElement>(new Uri(remoteAddress), options);
            ImplicitlyWait = TimeoutDefault;
            //TODO var a = Driver.FindElementByClassName("android.widget.FrameLayout");
        }

        /// <summary>
        /// Gets or sets underlying <see cref="AppiumDriver{AndroidElement}"/> instance.
        /// </summary>
        public AppiumDriver<AndroidElement> Driver { get; set; }

        /// <summary>
        /// Gets or sets implicit timeout of waiting for specified element to be existed in elements tree.
        /// </summary>
        public TimeSpan ImplicitlyWait
        {
            get => Driver.Manage().Timeouts().ImplicitWait;

            set => Driver.Manage().Timeouts().ImplicitWait = value;
        }

        /// <summary>
        /// Close android driver instance and associated application.
        /// </summary>
        public void Close()
        {
            Logger.Instance.Log(LogLevel.Debug, "Close driver");
            Driver.Quit();
        }
    }
}
