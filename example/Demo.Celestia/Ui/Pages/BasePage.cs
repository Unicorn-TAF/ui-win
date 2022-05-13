using Demo.Celestia.Ui.Common;
using OpenQA.Selenium;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Demo.Celestia.Ui.Pages
{
    public abstract class BasePage : WebPage
    {
        /// <summary>
        /// Initializes new instance of <see cref="BasePage"/> with webdriver intance and page info data.
        /// Page info data could be specified either by <see cref="PageInfoAttribute"/> or via constructor.
        /// </summary>
        /// <param name="driver"></param>
        public BasePage(IWebDriver driver, string subUrl, string title) : base(driver, subUrl, title)
        {
        }

        public BasePage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Each control could have name (specified in <see cref="NameAttribute"/>). 
        /// This can help to have more readable code, logs, reports and error messages from matchers.
        /// </summary>
        [ById("header"), Name("Page header")]
        public HeaderBlock Header { get; set; }

        /// <summary>
        /// Initializae complex PageObject element.
        /// If locator is not specified explicitly, then default locator is used (if any).
        /// </summary>
        [Name("Page footer")]
        public FooterBlock Footer { get; set; }
    }
}
