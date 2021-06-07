using NUnit.Framework;
using System.Windows.Forms;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UnitTests.UI.Win
{
    [TestFixture]
    public class WinControls
    {
        private static WinControl control;

        [OneTimeSetUp]
        public static void Setup() =>
            control = new Pane(WinDriver.Instance.SearchContext);

        [OneTimeTearDown]
        public static void TearDown() =>
            control = null;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BoundingRectangle property")]
        public void TestGuiControlBoundingRectangleProperty() =>
            Assert.AreEqual(Screen.PrimaryScreen.Bounds, control.BoundingRectangle);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Location property")]
        public void TestGuiControlLocationProperty() =>
            Assert.AreEqual(new System.Drawing.Point(0, 0), control.Location);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Visible property")]
        public void TestGuiControlVisibileProperty() =>
            Assert.IsTrue(control.Visible);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Enabled property")]
        public void TestGuiControlEnabledProperty() =>
            Assert.IsTrue(control.Enabled);
    }
}
