using Demo.Celestia.Ui.Pages;
using Demo.StepsInjection;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Web;

namespace Demo.Celestia.Steps
{
    /// <summary>
    /// Represents high-level steps for website.
    /// To make steps be able to use events subscriptions it's necessary to add <see cref="StepsClassAttribute"/>.
    /// </summary>
    [StepsClass]
    public class StepsCelestia
    {
        private CelestiaSite celestia;

        /// <summary>
        /// Gets web page from website cache.
        /// If page was already initializaed and called before the same instance is used further.
        /// </summary>
        private HomePage Home => celestia.GetPage<HomePage>();

        [Step("Open Celestia Web site in {0} browser")]
        public CelestiaSite Open(BrowserType browser)
        {
            celestia = new CelestiaSite(browser);
            celestia.Open();
            Home.WaitForLoading(Timeouts.PageLoadTimeout);

            return celestia;
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
            celestia.Driver.Close();
    }
}
