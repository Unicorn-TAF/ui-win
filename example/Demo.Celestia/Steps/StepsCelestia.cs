using Demo.Celestia.Ui.Pages;
using Demo.StepsInjection;
using OpenQA.Selenium.Interactions;
using System;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Utility.Synchronization;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;

namespace Demo.Celestia.Steps
{
    /// <summary>
    /// Represents high-level steps for website.
    /// To make steps be able to use events subscriptions it's necessary to add <see cref="StepsClassAttribute"/>.
    /// </summary>
    [StepsClass]
    public class StepsCelestia
    {
        public CelestiaSite Celestia => CelestiaSite.Instance;

        /// <summary>
        /// Gets web page from website cache.
        /// If page was already initializaed and called before the same instance is used further.
        /// </summary>
        private HomePage Home => Celestia.GetPage<HomePage>();

        [Step("Open Celestia Web site in {0} browser")]
        public void Open(BrowserType browser)
        {
            WebDriver.Instance = new DesktopWebDriver(browser);
            Celestia.ResetPagesCache();
            Celestia.Open();

            var actions = new Actions(WebDriver.Instance.SeleniumDriver);

            // During fade out animation nothing is interactable, need to wait for interaction is available.
            new DefaultWait(TimeSpan.FromSeconds(10))
                .Until(() =>
                {
                    actions.MoveToElement(Home.HomeLink.Instance).Perform();
                    bool result = !Home.HomeLink.Instance.GetCssValue("color").Equals("rgba(255, 255, 255, 1)");
                    actions.MoveByOffset(0, 0).Perform();
                    return result;
                });
        }

        /// <summary>
        /// Example of step with description and parmeters (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// With placeholders parameters could be substitured into description.
        /// </summary>
        [Step("Select '{0}' from main menu")]
        public void SelectMenu(string destination) =>
            Home
            .Header.GetNavItem(destination)
            .Click();

        /// <summary>
        /// Example of step with description (though <see cref="StepAttribute"/>).
        /// After subscription to test events it's possible to use attribute for reporting needs for example.
        /// </summary>
        [Step("Close Browser")]
        public void CloseBrowser() =>
            WebDriver.Close();
    }
}
