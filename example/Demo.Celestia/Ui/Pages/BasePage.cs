using Demo.Celestia.Ui.Common;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;

namespace Demo.Celestia.Ui.Pages
{
    public abstract class BasePage : WebPage
    {
        public BasePage(string subUrl, string title) : base(WebDriver.Instance.SeleniumDriver, subUrl, title)
        {
        }

        [ById("header"), Name("Page heder")]
        public HeaderBlock Header { get; set; }

        [ById("footer"), Name("Page footer")]
        public FooterBlock Footer { get; set; }
    }
}
