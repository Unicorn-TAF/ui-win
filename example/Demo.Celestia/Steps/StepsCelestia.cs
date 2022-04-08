using Demo.Celestia.Ui.Pages;
using Demo.StepsInjection;
using System.Threading;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Demo.Celestia.Steps
{
    /// <summary>
    /// Represents high-level steps for website.
    /// To make steps be able to use events subscriptions it's necessary to add StepsClass attribute.
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
            Celestia.Open();
            Home.Find<WebControl>(ByLocator.Css("ul.dropotron[style *= z-index]"));
            Thread.Sleep(3000); // Wait for animation (nothing changes in html source during the animation).
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
