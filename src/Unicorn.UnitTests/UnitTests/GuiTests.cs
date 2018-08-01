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
            var app = new CharmapApplication(@"C:\Windows\System32\", "charmap.exe");
            app.Start();
            app.Window.ButtonSelect.Click();
        }
    }
}
