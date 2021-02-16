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
    [PageInfo("http://labs.creative-area.net/jquery.datagrid/demo/")]
    public class JqueryDataGridPage : WebPage
    {
        public JqueryDataGridPage(OpenQA.Selenium.IWebDriver driver) : base(driver)
        {
        }

        [ById("dg-demo-static-data")]
        [DefineGrid(GridElement.Header, Using.WebTag, "th")]
        [DefineGrid(GridElement.Row, Using.WebCss, "tbody > tr")]
        [DefineGrid(GridElement.Cell, Using.WebCss, "tbody > tr > td")]
        public DynamicDataGrid DataGrid { get; set; }

        public void WaitForLoading() =>
            DataGrid.Wait(Until.Visible);
    }
}
