using NUnit.Framework;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.PageObject;
using Unicorn.UnitTests.UI.Gui.Win;

namespace Unicorn.UnitTests.UI.Tests.Win
{
    [TestFixture]
    public class WinDynamicDialog : WinTestsBase
    {
        private CharmapApplication charmap;

        [SetUp]
        public void Setup()
        {
            charmap = new CharmapApplication();
            charmap.Start();
        }

        [TearDown]
        public void TearDown() =>
            charmap.Close();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog close")]
        public void TestDialogClose()
        {
            charmap.WindowDynamic.Close();
            Assert.IsFalse(charmap.WindowDynamic.Exists());
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog click button by name")]
        public void TestDialogClickButtonByName()
        {
            charmap.WindowDynamic.ClickButton("Select");
            charmap.WindowDynamic.Find<Button>(ByLocator.Name("Copy")).Wait(Until.Enabled);
        }
    }
}
