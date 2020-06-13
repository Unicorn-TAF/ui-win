using NUnit.Framework;
using Unicorn.UnitTests.Gui.Win;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class WinPageObject : NUnitTestRunner
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
        [Test(Description = "Check private control field with default locator")]
        public void TestPrivateControlFieldWithDefaultLocator() =>
            Assert.IsTrue(charmap.Window.ButtonCopyDefaultLocatorGetter.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check public control property with default locator")]
        public void TestPublicControlPropertyWithDefaultLocator() =>
            Assert.IsTrue(charmap.Window.ButtonCopyDefaultLocator.Visible);
    }
}
