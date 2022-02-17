using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;

namespace Unicorn.UI.Web.Driver
{
    internal static class WebDriverFactory
    {
        internal static IWebDriver Get(BrowserType browser)
        {
            switch (browser)
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

        internal static IWebDriver Get(BrowserType browser, DriverOptions options)
        {
            switch (browser)
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

        internal static BrowserType GetBrowserType(IWebDriver seleniumDriver)
        {
            switch (seleniumDriver)
            {
                case ChromeDriver _:
                    return BrowserType.Chrome;

                case InternetExplorerDriver _:
                    return BrowserType.IE;

                case FirefoxDriver _:
                    return BrowserType.Firefox;

                case OperaDriver _:
                    return BrowserType.Opera;

                case EdgeDriver _:
                    return BrowserType.Edge;

                default:
                    throw new NotSupportedException("Selenium driver type is not supported: " + seleniumDriver.GetType());
            }
        }
    }
}
