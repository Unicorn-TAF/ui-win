using OpenQA.Selenium;

namespace Unicorn.UI.Web.Driver
{
    /// <summary>
    /// Represents Driver for Desktop version of bworser and allows to perform search of elements in web pages.
    /// </summary>
    public class DesktopWebDriver : WebDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type, driver options and window maximize state.
        /// </summary>
        public DesktopWebDriver(BrowserType browser, DriverOptions driverOptions, bool maximize)
        {
            Browser = browser;

            SeleniumDriver = driverOptions == null ? 
                WebDriverFactory.Get(browser) :
                WebDriverFactory.Get(browser, driverOptions);

            if (maximize)
            {
                SeleniumDriver.Manage().Window.Maximize();
            }

            ImplicitlyWait = TimeoutDefault;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type and window maximize state and without driver options.
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="maximize"></param>
        public DesktopWebDriver(BrowserType browser, bool maximize) : this(browser, null, maximize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class with specified browser type and maximized window.
        /// </summary>
        /// <param name="browser"></param>
        public DesktopWebDriver(BrowserType browser) : this(browser, null, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopWebDriver"/> class based on existing instance of <see cref="IWebDriver"/>.
        /// </summary>
        /// <param name="webDriverInstance">IWebDriver instance</param>
        public DesktopWebDriver(IWebDriver webDriverInstance)
        {
            SeleniumDriver = webDriverInstance;
            ImplicitlyWait = TimeoutDefault;
            Browser = WebDriverFactory.GetBrowserType(webDriverInstance);
        }
    }
}
