using System;
using NUnit.Framework;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Desktop.Driver;
using Unicorn.UnitTests.Gui;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class GuiPageObjectTests : NUnitTestRunner
    {
        private static CharmapApplication Charmap;

        [OneTimeSetUp]
        public static void Setup()
        {
            Charmap = new CharmapApplication(@"C:\Windows\System32\", "charmap.exe");
            Charmap.Start();
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that not existing controls don't brake page object initialization")]
        public void TestGuiPageObjectNotExistingControlsDontBrakePageObjectInitialization()
        {
            Assert.IsTrue(Charmap.Window.Visible);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check nested controls initialization")]
        public void TestGuiPageObjectNestedControlsInitialization()
        {
            Assert.IsTrue(Charmap.Window.ButtonCopy.Visible);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check non public controls initialization")]
        public void TestGuiPageObjectNonPublicControlsInitialization()
        {
            Assert.IsTrue(Charmap.Window.SelectButton.Visible);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check for basic control search from initialized parent container")]
        public void TestGuiPageObjectBasicControlSearchFromInitializedContainer()
        {
            Assert.IsTrue(Charmap.Window.ButtonHelp.Visible);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that page object initialized controls are cached")]
        public void TestGuiPageObjectInitializedControlsAreNotCached()
        {
            Assert.IsFalse(Charmap.Window.ButtonCopy.Cached);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check that controls found by base search are cached")]
        public void TestGuiPageObjectBaseSearchedControlsAreCached()
        {
            Assert.IsTrue(Charmap.Window.ButtonHelp.Cached);
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check Call for not existing control")]
        public void TestGuiPageObjectNotExistingControl()
        {
            var originalWait = GuiDriver.Instance.ImplicitlyWait;
            GuiDriver.Instance.ImplicitlyWait = TimeSpan.FromMilliseconds(10);

            try
            {
                var visible = Charmap.FakeWindow.Visible;
                Assert.Fail();
            }
            catch (ControlNotFoundException)
            {
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

        [OneTimeTearDown]
        public static void TearDown()
        {
            Charmap.Close();
        }
    }
}
