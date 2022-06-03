using NUnit.Framework;
using Unicorn.UnitTests.UI.Gui.Win;

namespace Unicorn.UnitTests.UI.Tests.Win
{
    [TestFixture]
    public class WinDynamicDropdown : WinTestsBase
    {
        private static CharmapApplication charmap;

        [OneTimeSetUp]
        public static void Setup()
        {
            charmap = new CharmapApplication();
            charmap.Start();
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            charmap.Close();
            charmap = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "No selection if value was already selected")]
        public void TestNoSelectionIfValueWasAlreadySelected()
        {
            charmap.Window.DDropdown.Select("Tahoma");
            var isSelectionWasMade = charmap.Window.DDropdown.Select("Tahoma");
            Assert.IsFalse(isSelectionWasMade);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Option selection")]
        public void TestOptionSelection()
        {
            var newValue = "Consolas";
            charmap.Window.DDropdown.Select("Tahoma");
            var isSelectionWasMade = charmap.Window.DDropdown.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
            Assert.AreEqual(newValue, charmap.Window.DDropdown.SelectedValue);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check collapse default state")]
        public void TestCollapseDefaultState() =>
            Assert.IsFalse(charmap.Window.DDropdown.Expanded);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check expanding")]
        public void TestExpanding()
        {
            charmap.Window.DDropdown.Expand();
            Assert.True(charmap.Window.DDropdown.Expanded);
            Assert.IsTrue(charmap.Window.DDropdown.GetOptions().Count > 50);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Dropdown with no input")]
        public void TestDropdownWithNoInput()
        {
            var newValue = "Tahoma";
            var isSelectionWasMade = charmap.Window.DDropdownNoInput.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
            isSelectionWasMade = charmap.Window.DDropdownNoInput.Select(newValue);
            Assert.IsTrue(isSelectionWasMade);
        }
    }
}
