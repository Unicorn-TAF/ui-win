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

        public BasePage() : base(WebDriver.Instance.SeleniumDriver)
        {

        }

        /// <summary>
        /// Each control could have name (specified in <see cref="NameAttribute"/>). 
        /// This can help to have more readable code, logs, reports and error messages from matchers.
        /// </summary>
        [ById("header"), Name("Page heder")]
        public HeaderBlock Header { get; set; }

        /// <summary>
        /// Initializae complex PageObject element.
        /// If locator is not specified explicitly, then default locator is used (if any).
        /// </summary>
        [Name("Page footer")]
        public FooterBlock Footer { get; set; }
    }
}
