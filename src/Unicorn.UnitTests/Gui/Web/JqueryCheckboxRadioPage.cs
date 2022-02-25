using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.Gui.Web
{
    [PageInfo("https://jqueryui.com/checkboxradio/")]
    public class JqueryCheckboxRadioPage : WebPage
    {
        public JqueryCheckboxRadioPage(OpenQA.Selenium.IWebDriver driver) : base(driver)
        {
        }

        [ById("checkbox-1")]
        public Checkbox JqCheckbox { get; set; }

        [ById("radio-1")]
        public Radio JqRadio { get; set; }

        public void WaitForLoading()
        {
            WebDriver.Instance.SeleniumDriver.SwitchTo().Frame(0);
            JqCheckbox.Wait(Until.Visible);
        }
    }
}
