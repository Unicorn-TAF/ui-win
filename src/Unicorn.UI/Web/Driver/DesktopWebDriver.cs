using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;

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

            SeleniumDriver = driverOptions == null ? GetInstance() : GetInstance(driverOptions);

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
        }

        private IWebDriver GetInstance()
        {
            switch (Browser)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver();
                case BrowserType.IE:
                    return new InternetExplorerDriver();
                case BrowserType.Firefox:
                    return new FirefoxDriver();
                case BrowserType.Opera:
                    return new OperaDriver();
                case BrowserType.Edge:
                    return new EdgeDriver();
                default:
                    return null;
            }
        }

        private IWebDriver GetInstance(DriverOptions options)
        {
            switch (Browser)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver((ChromeOptions)options);
                case BrowserType.IE:
                    return new InternetExplorerDriver((InternetExplorerOptions)options);
                case BrowserType.Firefox:
                    return new FirefoxDriver((FirefoxOptions)options);
                case BrowserType.Opera:
                    return new OperaDriver((OperaOptions)options);
                case BrowserType.Edge:
                    return new EdgeDriver((EdgeOptions)options);
                default:
                    return null;
            }
        }
    }
}
