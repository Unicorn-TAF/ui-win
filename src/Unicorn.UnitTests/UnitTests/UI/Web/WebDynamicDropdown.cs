using NUnit.Framework;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.Gui.Web;

namespace Unicorn.UnitTests.UI.Web
{
    [TestFixture]
    public class WebDynamicDropdown
    {
        private static JquerySelectPage page;

        [OneTimeSetUp]
        public static void Setup()
        {
            WebDriver.Instance = new DesktopWebDriver(BrowserType.Chrome, true);
            page = new JquerySelectPage(WebDriver.Instance.SeleniumDriver);
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
        [Test(Description = "No selection if value was already selected")]
        public void TestNoSelectionIfValueWasAlreadySelected()
        {
            page.Dropdown.Select("Medium");
            var isSelectionWasMade = page.Dropdown.Select("Medium");
            Assert.IsFalse(isSelectionWasMade);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Option selection")]
        public void TestOptionSelection()
        {
            var newValue = "Faster";
            var isSelectionWasMade = page.Dropdown.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
            Assert.AreEqual(newValue, page.Dropdown.SelectedValue);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check collapse default state")]
        public void TestCollapseDefaultState() =>
            Assert.IsFalse(page.Dropdown.Expanded);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check expanding")]
        public void TestExpanding()
        {
            page.Dropdown.Expand();
            Assert.True(page.Dropdown.Expanded);
            Assert.AreEqual(5, page.Dropdown.GetOptions().Count);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Dropdown with no input")]
        public void TestDropdownWithNoInput()
        {
            var newValue = "Faster";
            var isSelectionWasMade = page.DropdownNoInput.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
            isSelectionWasMade = page.DropdownNoInput.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
        }
    }
}
