using OpenQA.Selenium;
using System.Collections.Generic;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Core.PageObject.By;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.PageObject;
using Unicorn.UI.Web.PageObject.Attributes;

namespace Unicorn.UnitTests.UI.Gui.Web
{
    [PageInfo("https://jqueryui.com/resources/demos/checkboxradio/default.html")]
    public class JqueryCheckboxRadioPage : WebPage
    {
        internal const string RadioButtonName = "Radiobutton";

        public JqueryCheckboxRadioPage(IWebDriver driver) : base(driver)
        {
        }

        [ById("checkbox-1")]
        public Checkbox JqCheckbox { get; set; }

        [ById("checkbox-2")]
        public Checkbox JqCheckboxToCheck1 { get; set; }

        [ById("checkbox-3")]
        public Checkbox JqCheckboxToCheck2 { get; set; }

        [Name(RadioButtonName)]
        [ById("radio-1")]
        public Radio JqRadio { get; set; }

        [ById("radio-2")]
        public Radio JqRadioToSelect { get; set; }

        [ById("radio-notexisting")]
        public Radio NotExistingRadio { get; set; }

        public CustomCheckbox CheckboxCustom { get; set; }

        public IList<CustomCheckbox> CheckboxesCustomList { get; set; }
    }
}
