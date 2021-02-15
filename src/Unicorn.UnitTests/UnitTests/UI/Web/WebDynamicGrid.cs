using NUnit.Framework;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.Gui.Web;

namespace Unicorn.UnitTests.UnitTests.UI.Web
{
    [TestFixture]
    public class WebDynamicGrid
    {
        private static JqueryDataGridPage page;

        [OneTimeSetUp]
        public static void Setup()
        {
            WebDriver.Instance = new DesktopWebDriver(BrowserType.Chrome, true);
            page = new JqueryDataGridPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            page = null;
            WebDriver.Close();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get header by name")]
        public void TestGetHeaderByName() =>
            Assert.AreEqual("Continent", page.DataGrid.GetColumnHeader("Continent").Text);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get rows count")]
        public void TestGetRowsCount() =>
            Assert.AreEqual(20, page.DataGrid.RowsCount);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get specific row")]
        public void TestGetSpecificRow() =>
            Assert.AreEqual("South America", page.DataGrid.GetRow("Name", "Argentina").GetCell(0).Data);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get row by index")]
        public void TestGetRowByIndex() =>
            Assert.AreEqual("Angola", page.DataGrid.GetRow(2).GetCell(1).Data);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get specific cell")]
        public void TestGetSpecificCell() =>
            Assert.AreEqual("78000", page.DataGrid.GetCell("Name", "Andorra", "Population").Data);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Get cell by indexes")]
        public void TestGetCellByIndexes() =>
            Assert.AreEqual("83600.00", page.DataGrid.GetCell(7, 3).Data);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has column (positive)")]
        public void TestHasColumnPositive() =>
            Assert.IsTrue(page.DataGrid.HasColumn("Surface"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has column (negative)")]
        public void TestHasColumnNegative() =>
            Assert.IsFalse(page.DataGrid.HasColumn("PopulationWee"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has row (positive)")]
        public void TestHasRowPositive() =>
            Assert.IsTrue(page.DataGrid.HasRow("Population", "0"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has row (negative)")]
        public void TestHasRowNegative() =>
            Assert.IsFalse(page.DataGrid.HasRow("Population", "-900"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has cell (3 params positive)")]
        public void TestHasCell3ParamsPositive() =>
            Assert.IsTrue(page.DataGrid.HasCell("Population", "0", "Continent"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has cell (3 params negative)")]
        public void TestHasCell3ParamsNegative() =>
            Assert.IsFalse(page.DataGrid.HasCell("Population", "0", "Contsadsainent"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has cell (4 params positive)")]
        public void TestHasCell4ParamsPositive() =>
            Assert.IsTrue(page.DataGrid.HasCell("Population", "0", "Continent", "Antarctica"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Has cell (4 params negative)")]
        public void TestHasCell4ParamsNegative() =>
            Assert.IsFalse(page.DataGrid.HasCell("Population", "0", "Continent", "Weee"));
    }
}
