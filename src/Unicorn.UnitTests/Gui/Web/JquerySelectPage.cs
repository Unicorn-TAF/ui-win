using OpenQA.Selenium;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Web.Controls.Dynamic;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.Gui.Web
{
    [PageInfo("https://jqueryui.com/selectmenu/", "Selectmenu | jQuery UI")]
    public class JquerySelectPage : WebPage
    {
        public JquerySelectPage(IWebDriver driver) : base(driver)
        {
        }

        [ById("speed-button")]
        [DefineDropdown(DropdownElement.ValueInput, Using.WebCss, ".ui-selectmenu-text")]
        [DefineDropdown(DropdownElement.ExpandCollapse, Using.WebCss, ".ui-selectmenu-icon")]
        [DefineDropdown(DropdownElement.OptionsFrame, Using.WebXpath, "//div[contains(@class, 'ui-selectmenu-open')]")]
        [DefineDropdown(DropdownElement.Option, Using.WebXpath, "//div[contains(@class, 'ui-selectmenu-open')]//div[@role = 'option']")]
        public DynamicDropdown Dropdown { get; set; }

        [ById("speed-button")]
        [DefineDropdown(DropdownElement.ExpandCollapse, Using.WebCss, ".ui-selectmenu-icon")]
        [DefineDropdown(DropdownElement.OptionsFrame, Using.WebXpath, "//div[contains(@class, 'ui-selectmenu-open')]")]
        [DefineDropdown(DropdownElement.Option, Using.WebXpath, "//div[contains(@class, 'ui-selectmenu-open')]//div[@role = 'option']")]
        public DynamicDropdown DropdownNoInput { get; set; }

        public void WaitForLoading()
        {
            (SearchContext as IWebDriver).SwitchTo().Frame(0);
            Dropdown.Wait(Until.Visible);
        }
    }
}
