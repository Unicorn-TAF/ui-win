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
            ////Do.CharMap.StartApplication(ExePath + "charmap.exe");
        }

        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("test")]
        [Test("Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            Do.IOS.NavigateTo("http://www.bing.com");
            ////Do.iOS.SearchFor("bla-bla-bla");
            ////Do.Android.NavigateTo("http://www.bing.com");
            ////Do.Android.SearchFor("bla-bla-bla");
            ////Do.CharMap.DoSomething("Calibri");
            ////Do.YandexMarket.OpenPortal(PortalUrl);
            ////Bug("76237").YandexMarket.DoSomeActions();
        }

        [AfterTest]
        public void TearDown()
        {
            Logger.Instance.Info("After Test: close app and browser");
            ////Do.CharMap.CloseApplication();
            ////Do.YandexMarket.CloseBrowser();
        }
    }
}
