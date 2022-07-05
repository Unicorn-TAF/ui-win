using OpenQA.Selenium;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.UI.Gui.Web
{
    [PageInfo("https://jqueryui.com/resources/demos/checkboxradio/default.html")]
    public class JqueryCheckboxRadioPage : WebPage
    {
        public JqueryCheckboxRadioPage(IWebDriver driver) : base(driver)
        {
        }

        [ById("checkbox-1")]
        public Checkbox JqCheckbox { get; set; }

        [ById("radio-1")]
        public Radio JqRadio { get; set; }
    }
}
