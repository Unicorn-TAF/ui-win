using AspectInjector.Broker;
using Demo.Celestia.Ui.Pages;
using System.Threading;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Demo.Celestia.Steps
{
    [Inject(typeof(StepsEvents))]
    public class StepsCelestia
    {
        public CelestiaSite Celestia => CelestiaSite.Instance;

        [Step("Open Celestia Web site in {0} browser")]
        public void Open(BrowserType browser)
        {
            WebDriver.Instance = new DesktopWebDriver(browser);
            Celestia.Open();
            Celestia.GetPage<HomePage>()
                .Find<WebControl>(ByLocator.Css("ul.dropotron[style *= z-index]"));
            Thread.Sleep(3000); // Wait for animation (nothing changes in html source during the animation).
        }

        [Step("Select '{0}' from main menu")]
        public void SelectMenu(string destination) =>
            Celestia.GetPage<HomePage>()
            .Header.GetNavItem(destination)
            .Click();

        [Step("Close Browser")]
        public void CloseBrowser() =>
            WebDriver.Close();
    }
}
