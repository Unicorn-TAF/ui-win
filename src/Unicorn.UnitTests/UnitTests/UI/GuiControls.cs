using NUnit.Framework;
using System.Windows.Forms;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Desktop.Controls.Typified;
using Unicorn.UI.Desktop.Driver;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class GuiControls
    {
        private static GuiControl control;

        [OneTimeSetUp]
        public static void Setup() =>
            control = new Pane(GuiDriver.Instance.SearchContext);

        [OneTimeTearDown]
        public static void TearDown() =>
            control = null;

        [Ignore("Hangs explorer")]
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check BoundingRectangle property")]
        public void TestGuiControlBoundingRectangleProperty() =>
            Assert.AreEqual(SystemInformation.VirtualScreen, control.BoundingRectangle);

        [Ignore("Hangs explorer")]
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Location property")]
        public void TestGuiControlLocationProperty() =>
            Assert.AreEqual(new System.Drawing.Point(0, 0), control.Location);

        [Ignore("Hangs explorer")]
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Visible property")]
        public void TestGuiControlVisibileProperty() =>
            Assert.IsTrue(control.Visible);

        [Ignore("Hangs explorer")]
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Enabled property")]
        public void TestGuiControlEnabledProperty() =>
            Assert.IsTrue(control.Enabled);
    }
}
