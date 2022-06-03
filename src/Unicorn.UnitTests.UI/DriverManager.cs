using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UnitTests.UI
{
    internal static class DriverManager
    {
        internal static DesktopWebDriver GetDriverInstance()
        {
            IWebDriver driver = new ChromeDriver(GetChromeOptions());

            return new DesktopWebDriver(driver);
        }

        private static ChromeOptions GetChromeOptions()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArguments(
                "allow-insecure-localhost",
                "ignore-certificate-errors",
                "ignore-ssl-errors=yes",
                "disable-extensions",
                "disable-infobars",
                "no-sandbox",
                "disable-impl-side-painting",
                "enable-gpu-rasterization",
                "force-gpu-rasterization",
                "headless",
                "--window-size=1920x1080");

            return options;
        }
    }
}
