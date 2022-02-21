using NUnit.Framework;
using System.Diagnostics;
using System.Drawing;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Win.Controls.Typified;
using Unicorn.UI.Win.Driver;
using Unicorn.UI.Win.UserInput;

namespace Unicorn.UnitTests.UI.Win
{
    [TestFixture]
    public class UserInput
    {
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Mouse movement")]
        public void TestMouseMovement()
        {
            Point coordinates = new Point(50, 50);
            Mouse.Instance.Location = coordinates;
            Assert.AreEqual(coordinates, Mouse.Instance.Location);
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
