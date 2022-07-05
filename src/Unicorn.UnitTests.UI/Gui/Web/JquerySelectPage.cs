using OpenQA.Selenium;
using System.Collections.Generic;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls.Dynamic;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.UI.Gui.Web
{
    [PageInfo("https://jqueryui.com/resources/demos/selectmenu/default.html", "jQuery UI Selectmenu - Default functionality")]
    public class JquerySelectPage : WebPage
    {
        [Find(Using.WebCss, "fieldset select")]
        private IList<Dropdown> dropdownsList;

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
        
        [Find(Using.WebCss, "fieldset select")]
        public IList<Dropdown> DropdownsList { get; set; }

        public IList<Dropdown> DropdownsListwithBackingField => dropdownsList;
    }
}
