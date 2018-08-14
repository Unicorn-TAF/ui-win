using NUnit.Framework;
using Unicorn.UnitTests.Gui;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests
{
    [TestFixture]
    public class GuiTests : NUnitTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Gui test")]
        public void TestGui()
        {
            var app = new WinCharmapApplication(@"C:\Windows\System32\", "charmap.exe");
            app.Start();
            app.Window.ButtonSelect.Click();
        }

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Gui test2")]
        public void TestGui2()
        {
            var app = new WinCharmapApplication(@"C:\Windows\System32\", "charmap.exe");
            app.Start();
            app.Window.DropdownFonts.Select("Cambria");
        }
    }
}
