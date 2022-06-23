using NUnit.Framework;
using Unicorn.UnitTests.UI.Gui.Android;

namespace Unicorn.UnitTests.UI.Tests.Mobile
{
    [TestFixture]
    public class AndroidTests
    {
        private AndroidDialerApi25 app;

        [SetUp]
        public void SetUp()
        {
            app = new AndroidDialerApi25("http://127.0.0.1:4723/wd/hub", "device");
            app.Open();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Android UI basics")]
        public void TestAndroidUiBasics()
        {
            
            app.Container.ActionBar.ButtonHistory.Click();
            app.Container.ButtonDialPad.Click();
            app.Container.DialPad.GetButton("#").Click();
            app.Container.DialPad.GetButton("2").Click();

            Assert.IsTrue(Unicorn.UI.Core.Matchers.UI.Control.HasText("#2")
                .Matches(app.Container.DialPad.InputNumber));
        }

        [TearDown]
        public void TearDown()
        {
            app.Driver.Close();
        }
    }
}
