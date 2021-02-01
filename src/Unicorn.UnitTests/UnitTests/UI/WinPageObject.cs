using NUnit.Framework;
using System;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Win.Driver;
using Unicorn.UnitTests.Gui.Win;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class WinPageObject
    {
        private static CharmapApplication charmap;

        [OneTimeSetUp]
        public static void Setup()
        {
            charmap = new CharmapApplication();
            charmap.Start();
        }

        [OneTimeTearDown]
        public static void TearDown() =>
            charmap.Close();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that not existing controls don't brake page object initialization")]
        public void TestWinPageObjectNotExistingControlsDontBrakePageObjectInitialization() =>
            Assert.IsTrue(charmap.Window.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check nested controls initialization")]
        public void TestWinPageObjectNestedControlsInitialization() =>
            Assert.IsTrue(charmap.Window.ButtonCopy.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check non public controls initialization")]
        public void TestWinPageObjectNonPublicControlsInitialization() =>
            Assert.IsTrue(charmap.Window.SelectButton.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check typified locators work")]
        public void TestTypifiedLocators() =>
            Assert.IsTrue(charmap.Window.ButtonSelectLocatedByName.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check initialization of controls as class fields")]
        public void TestWinPageObjectInitializationOfControlsAsClassFields() =>
            Assert.IsTrue(charmap.Window.GetCopyButtonFromField().Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check for basic control search from initialized parent container")]
        public void TestWinPageObjectBasicControlSearchFromInitializedContainer() =>
            Assert.IsTrue(charmap.Window.ButtonHelp.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ability to search for generic WinControl")]
        public void TestWinPageObjectAbilityToSearchForGenericWinControl() =>
            Assert.IsTrue(charmap.Window.ButtonHelpGeneric.Text.Equals("Help"));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that page object initialized controls are cached")]
        public void TestWinPageObjectInitializedControlsAreNotCached() =>
            Assert.IsFalse(charmap.Window.ButtonCopy.Cached);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that controls found by base search are cached")]
        public void TestWinPageObjectBaseSearchedControlsAreCached() =>
            Assert.IsTrue(charmap.Window.ButtonHelp.Cached);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Call for not existing control")]
        public void TestWinPageObjectNotExistingControl()
        {
            var originalWait = WinDriver.Instance.ImplicitlyWait;
            WinDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(10);

            try
            {
                var enabled = charmap.FakeWindow.Enabled;
                Assert.Fail($"windows is enabled ({enabled})");
            }
            catch (ControlNotFoundException)
            {
                // this is positive scenario, nothing to do
            }
            catch (AssertionException)
            {
                Assert.Fail("Fake windows was found");
            }
            finally
            {
                WinDriver.Instance.ImplicitlyWait = originalWait;
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check private control field with default locator")]
        public void TestPrivateControlFieldWithDefaultLocator() =>
            Assert.IsTrue(charmap.Window.ButtonCopyDefaultLocatorGetter.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check public control property with default locator")]
        public void TestPublicControlPropertyWithDefaultLocator() =>
            Assert.IsTrue(charmap.Window.ButtonCopyDefaultLocator.Visible);
    }
}
