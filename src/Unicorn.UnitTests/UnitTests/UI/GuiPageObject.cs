using System;
using NUnit.Framework;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Desktop.Driver;
using Unicorn.UnitTests.Gui;
using Unicorn.UnitTests.Gui.Desktop;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class GuiPageObject : NUnitTestRunner
    {
        private static CharmapApplication charmap;

        [OneTimeSetUp]
        public static void Setup()
        {
            charmap = new CharmapApplication(@"C:\Windows\System32", "charmap.exe");
            charmap.Start();
        }

        [OneTimeTearDown]
        public static void TearDown() =>
            charmap.Close();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that not existing controls don't brake page object initialization")]
        public void TestGuiPageObjectNotExistingControlsDontBrakePageObjectInitialization() =>
            Assert.IsTrue(charmap.Window.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check nested controls initialization")]
        public void TestGuiPageObjectNestedControlsInitialization() =>
            Assert.IsTrue(charmap.Window.ButtonCopy.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check non public controls initialization")]
        public void TestGuiPageObjectNonPublicControlsInitialization() =>
            Assert.IsTrue(charmap.Window.SelectButton.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check typified locators work")]
        public void TestTypifiedLocators() =>
            Assert.IsTrue(charmap.Window.ButtonSelectLocatedByName.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check initialization of controls as class fields")]
        public void TestGuiPageObjectInitializationOfControlsAsClassFields() =>
            Assert.IsTrue(charmap.Window.GetCopyButtonFromField().Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check for basic control search from initialized parent container")]
        public void TestGuiPageObjectBasicControlSearchFromInitializedContainer() =>
            Assert.IsTrue(charmap.Window.ButtonHelp.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that page object initialized controls are cached")]
        public void TestGuiPageObjectInitializedControlsAreNotCached() =>
            Assert.IsFalse(charmap.Window.ButtonCopy.Cached);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that controls found by base search are cached")]
        public void TestGuiPageObjectBaseSearchedControlsAreCached() =>
            Assert.IsTrue(charmap.Window.ButtonHelp.Cached);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Call for not existing control")]
        public void TestGuiPageObjectNotExistingControl()
        {
            var originalWait = GuiDriver.Instance.ImplicitlyWait;
            GuiDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(10);

            try
            {
                var visible = charmap.FakeWindow.Visible;
                Assert.Fail($"windows is visible ({visible})");
            }
            catch (ControlNotFoundException)
            {
                // this is positive scenario, nothing to do
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                GuiDriver.Instance.ImplicitlyWait = originalWait;
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
