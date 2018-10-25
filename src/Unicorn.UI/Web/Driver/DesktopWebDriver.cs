using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Unicorn.UI.Web.Driver
{
    public class DesktopWebDriver : WebDriver
    {
        public DesktopWebDriver(BrowserType browser, DriverOptions driverOptions, bool maximize)
        {
            this.Browser = browser;

            Driver = driverOptions == null ? GetInstance() : GetInstance(driverOptions);

            if (maximize)
            {
                Driver.Manage().Window.Maximize();
            }

            this.ImplicitlyWait = this.TimeoutDefault;
        }

        public DesktopWebDriver(BrowserType browser, bool maximize) : this(browser, null, maximize)
        {
        }

        public DesktopWebDriver(BrowserType browser) : this(browser, false)
        {
        }

        public DesktopWebDriver() : this(BrowserType.Chrome)
        {
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
                default:
                    return null;
            }
        }
    }
}
