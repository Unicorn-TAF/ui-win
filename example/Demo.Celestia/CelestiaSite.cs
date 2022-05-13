using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia
{
    /// <summary>
    /// Describes https://celestia.space website (should inherit <see cref="WebSite"/>).
    /// </summary>
    public class CelestiaSite : WebSite
    {
        public const string SiteUrl = "https://celestia.space";

        /// <summary>
        /// Website constructor. Calls base constructor with address to website.
        /// </summary>
        public CelestiaSite(WebDriver driver) : base(driver, SiteUrl)
        {
        }

        /// <summary>
        /// Website constructor. Calls base constructor with address to website.
        /// </summary>
        public CelestiaSite(BrowserType browser) : base(browser, SiteUrl)
        {
        }
    }
}
