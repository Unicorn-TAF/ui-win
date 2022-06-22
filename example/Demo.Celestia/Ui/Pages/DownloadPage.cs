using System.Collections.Generic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Demo.Celestia.Ui.Pages
{
    /// <summary>
    /// Example of web page. 
    /// Page properties like relative url and title could be specified using <see cref="PageInfoAttribute"/>.
    /// Relative url property is implicitly used to navigate to the page from <see cref="WebSite"/> instance.
    /// Title property is implicitly used in page <see cref="Opened"/> 
    /// property indicating wherer the page is opened in browser or not.
    /// </summary>
    [PageInfo("download.html", "Celestia: Download")]
    public class DownloadPage : BasePage
    {
        public DownloadPage(OpenQA.Selenium.IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// List of elements could be found by using <see cref="FindList"/> method and 
        /// could not be initialized by Find attribute usage.
        /// </summary>
        public IList<WebControl> DownloadsList => FindList<WebControl>(ByLocator.Css("section#content i"));
    }
}
