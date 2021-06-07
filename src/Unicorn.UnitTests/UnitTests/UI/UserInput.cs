using NUnit.Framework;
using System.Diagnostics;
using System.Windows.Forms;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.UserInput;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;

namespace Unicorn.UnitTests.UI
{
    [TestFixture]
    public class UserInput
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Mouse movement")]
        public void TestMouseMovement()
        {
            var coordinate = 50;
            Mouse.Instance.Location = new System.Windows.Point(coordinate, coordinate);
            Assert.AreEqual(new System.Drawing.Point(coordinate, coordinate), Cursor.Position);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Keyboard hotkey")]
        public void TestKeyboardHotkey()
        {
            Keyboard.Instance.HoldKey(Keyboard.SpecialKeys.RightWin);
            Keyboard.Instance.Type("r");
            Keyboard.Instance.LeaveAllKeys();

            var appeared = WinDriver.Instance.TryGetChild<Window>(ByLocator.Name("Run"), 5000);

            Keyboard.Instance.PressSpecialKey(Keyboard.SpecialKeys.Escape);

            bool disappeared;
            var t = Stopwatch.StartNew();

            do
            {
                disappeared = !WinDriver.Instance.TryGetChild<Window>(ByLocator.Name("Run"), 100);
            }
            while (!disappeared && t.ElapsedMilliseconds < 5000);

            Assert.IsTrue(appeared, "Run process has not appeared");
            Assert.IsTrue(disappeared, "Run process has not disappeared");
        }
    }
}
