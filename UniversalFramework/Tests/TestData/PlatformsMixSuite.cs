using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Assertions.Matchers;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;
using Unicorn.UI.Core.Matchers;

namespace Tests.TestData
{
    [TestSuite("Tests for mixed platforms")]
    [Feature("Timeseries Analysis"), Feature("Yandex Market")]
    public class PlatformsMixSuite : BaseTestSuite
    {
        private const string ExePath = @"C:\Windows\System32\";
        private const string PortalUrl = @"https://market.yandex.ru/";

        [BeforeTest]
        public void BeforeTest()
        {
            Logger.Instance.Info("BeforeTest started");
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Gui")]
        [Test("Run Gui driver test")]
        public void GuiDriverTest()
        {
            Do.UI.CharMap.StartApplication(ExePath + "charmap.exe");
            Do.UI.CheckThat(Do.UI.CharMap.CharMap.InputCharactersToCopy, Control.HasAttribute("class").IsEqualTo("RICHEDIT50W"));
            Do.UI.CheckThat(Do.UI.CharMap.CharMap.ButtonCopy, Is.Not(Control.Enabled()));
            Do.UI.CharMap.SelectFont("Calibri");
            Do.UI.CharMap.CloseApplication();
        }

        [Skip]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Web")]
        [Test("Run Web driver test")]
        public void WebDriverTest()
        {
            Do.UI.YandexMarket.OpenPortal(PortalUrl);
            Bug("76237").UI.YandexMarket.DoSomeActions();
            Do.UI.YandexMarket.CloseBrowser();
        }

        [Skip]
        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Mobile")]
        [Test("Run mobile driver test")]
        public void SingleDriverTest()
        {
            Do.UI.IOS.NavigateTo("http://www.bing.com");
            Do.UI.IOS.SearchFor("bla-bla-bla");
            Do.UI.Android.NavigateTo("http://www.bing.com");
            Do.UI.Android.SearchFor("bla-bla-bla");
        }

        [AfterTest]
        public void TearDown()
        {
            Logger.Instance.Info("After Test");
        }
    }
}
