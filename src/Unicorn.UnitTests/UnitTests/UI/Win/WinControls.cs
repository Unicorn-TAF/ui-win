using NUnit.Framework;
using System.Drawing;
using Unicorn.UI.Win.Controls;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;
using Unicorn.UI.Win.WindowsApi;

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
            Assert.AreEqual(new Rectangle(new Point(0, 0), Screen.GetSize()), control.BoundingRectangle);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Location property")]
        public void TestGuiControlLocationProperty() =>
            Assert.AreEqual(new Point(0, 0), control.Location);

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
