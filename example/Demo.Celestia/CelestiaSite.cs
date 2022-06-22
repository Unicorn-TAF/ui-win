using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia
{
    /// <summary>
    /// Describes Celestia app website (should inherit <see cref="WebSite"/>).
    /// </summary>
    public class CelestiaSite : WebSite
    {
        /// <summary>
        /// Website constructor. Calls base constructor with address to website.
        /// </summary>
        public CelestiaSite(WebDriver driver, string siteUrl) : base(driver, siteUrl)
        {
        }

        /// <summary>
        /// Website constructor. Calls base constructor with address to website.
        /// </summary>
        public CelestiaSite(BrowserType browser, string siteUrl) : base(browser, siteUrl)
        {
        }
    }
}
