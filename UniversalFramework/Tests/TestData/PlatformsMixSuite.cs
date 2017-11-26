using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Tests for mixed platforms")]
    [Feature("Timeseries Analysis"), Feature("Yandex Market")]
    class PlatformsMixSuite : BaseTestSuite
    {
        const string EXE_PATH = @"C:\Windows\System32\";
        const string PORTAL_URL = @"https://market.yandex.ru/";

        [BeforeTest]
        public void BeforeTest()
        {
            Do.CharMap.StartApplication(EXE_PATH + "charmap.exe");
        }

        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("test")]
        [Test("Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            Do.CharMap.DoSomething("Calibri");
            Do.YandexMarket.OpenPortal(PORTAL_URL);
            Bug("76237").YandexMarket.DoSomeActions();
        }

        [AfterTest]
        public void TearDown()
        {
            Logger.Instance.Info("After Test: close app and browser");
            Do.CharMap.CloseApplication();
            Do.YandexMarket.CloseBrowser();
        }
    }
}
