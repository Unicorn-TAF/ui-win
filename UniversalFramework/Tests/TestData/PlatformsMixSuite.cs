using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

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

        [Skip]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Gui")]
        [Test("Run Gui driver test")]
        public void GuiDriverTest()
        {
            Do.CharMap.StartApplication(ExePath + "charmap.exe");
            Do.CharMap.DoSomething("Calibri");
            Do.CharMap.CloseApplication();
        }

        [Skip]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Web")]
        [Test("Run Web driver test")]
        public void WebDriverTest()
        {
            Do.YandexMarket.OpenPortal(PortalUrl);
            Bug("76237").YandexMarket.DoSomeActions();
            Do.YandexMarket.CloseBrowser();
        }

        [Skip]
        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Mobile")]
        [Test("Run mobile driver test")]
        public void SingleDriverTest()
        {
            Do.IOS.NavigateTo("http://www.bing.com");
            Do.IOS.SearchFor("bla-bla-bla");
            Do.Android.NavigateTo("http://www.bing.com");
            Do.Android.SearchFor("bla-bla-bla");
        }

        [AfterTest]
        public void TearDown()
        {
            Logger.Instance.Info("After Test");
        }
    }
}
