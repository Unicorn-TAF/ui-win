using NUnit.Framework;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.UI.Gui.Web;
using Uv = Unicorn.Taf.Core.Verification;
using Ui = Unicorn.UI.Core.Matchers.UI;

namespace Unicorn.UnitTests.UI.Tests
{
    [TestFixture]
    public class UiMatchersWeb : WebTestsBase
    {
        private static WebDriver webdriver;

        [OneTimeSetUp]
        public static void Setup() =>
            webdriver = DriverManager.GetDriverInstance();

        [OneTimeTearDown]
        public static void TearDown()
        {
            webdriver.SeleniumDriver.Quit();
            webdriver = null;
        }

        public JqueryDataGridPage OpenGridPage()
        {
            JqueryDataGridPage gridPage = NavigateToPage<JqueryDataGridPage>(webdriver.SeleniumDriver);
            gridPage.WaitForLoading();

            return gridPage;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check WindowHasTitleMatcher")]
        public void TestWindowHasTitleMatcherMatcher()
        {
            JqueryDialogPage dialogPage = NavigateToPage<JqueryDialogPage>(webdriver.SeleniumDriver);
            Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasTitle("Empty the recycle bin?"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check WindowHasTitleMatcher Negative")]
        public void TestWindowHasTitleMatcherMatcherNegative()
        {
            JqueryDialogPage dialogPage = NavigateToPage<JqueryDialogPage>(webdriver.SeleniumDriver);

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasTitle("weeee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ModalWindowHasTextMatcher")]
        public void TestModalWindowHasTextMatcher()
        {
            JqueryDialogPage dialogPage = NavigateToPage<JqueryDialogPage>(webdriver.SeleniumDriver);
            Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasText(
                "These items will be permanently deleted and cannot be recovered. Are you sure?"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ModalWindowHasTextMatcher Negative")]
        public void TestModalWindowHasTextMatcherNegative()
        {
            JqueryDialogPage dialogPage = NavigateToPage<JqueryDialogPage>(webdriver.SeleniumDriver);

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(dialogPage.Dialog, Ui.Window.HasText("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowsCountMatcher")]
        public void TestDataGridHasRowsCountMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRowsCount(20));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowsCountMatcher Negative")]
        public void TestDataGridHasRowsCountMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRowsCount(10));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowMatcher")]
        public void TestDataGridHasRowMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRow("Continent", "Asia"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasRowMatcher Negative")]
        public void TestDataGridHasRowMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasRow("Continent", "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasColumnMatcher")]
        public void TestDataGridHasColumnMatcher()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasColumn("Name"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasColumnMatcher Negative")]
        public void TestDataGridHasColumnMatcherNegative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasColumn("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher1")]
        public void TestDataGridHasCellWithTextMatcher1()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText("Continent", "Europe", "Name", "Albania"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher1 Negative")]
        public void TestDataGridHasCellWithTextMatcher1Negative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText("Continent", "Europe", "Name", "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher2")]
        public void TestDataGridHasCellWithTextMatcher2()
        {
            JqueryDataGridPage gridPage = OpenGridPage();
            Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText(0, 1, "Aruba"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DataGridHasCellWithTextMatcher2 Negative")]
        public void TestDataGridHasCellWithTextMatcher2Negative()
        {
            JqueryDataGridPage gridPage = OpenGridPage();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(gridPage.DataGrid, Ui.DataGrid.HasCellWithText(0, 1, "weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DropdownHasSelectedValueMatcher")]
        public void TestDropdownHasSelectedValueMatcher()
        {
            JquerySelectPage selectPage = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);
            Uv.Assert.That(selectPage.Dropdown, Ui.Dropdown.HasSelectedValue("Medium"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check DropdownHasSelectedValueMatcher Negative")]
        public void TestDropdownHasSelectedValueMatcherNegative()
        {
            JquerySelectPage selectPage = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(selectPage.Dropdown, Ui.Dropdown.HasSelectedValue("weee"));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxCheckedMatcher")]
        public void TestCheckboxCheckedMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);
            cboxPage.JqCheckboxToCheck1.JsClick();
            Uv.Assert.That(cboxPage.JqCheckboxToCheck1, Ui.Checkbox.Checked());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxCheckedMatcher Negative")]
        public void TestCheckboxCheckedMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.Checked());
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxHasCheckStateMatcher")]
        public void TestCheckboxHasCheckStateMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);
            Uv.Assert.That(cboxPage.JqCheckbox, Ui.Checkbox.HasCheckState(false));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check CheckboxHasCheckStateMatcher Negative")]
        public void TestCheckboxHasCheckStateMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);
            cboxPage.JqCheckboxToCheck2.JsClick();

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqCheckboxToCheck2, Ui.Checkbox.HasCheckState(false));
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check SelectedMatcher")]
        public void TestSelectedMatcher()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);
            cboxPage.JqRadioToSelect.JsClick();
            Uv.Assert.That(cboxPage.JqRadioToSelect, Ui.Control.Selected());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check SelectedMatcher Negative")]
        public void TestSelectedMatcherNegative()
        {
            JqueryCheckboxRadioPage cboxPage = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);

            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That(cboxPage.JqRadio, Ui.Control.Selected());
            });
        }
    }
}
