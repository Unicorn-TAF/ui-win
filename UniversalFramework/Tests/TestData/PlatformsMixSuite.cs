using ProjectSpecific.Steps;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Tests for mixed platforms")]
    [Feature("Timeseries Analysis"), Feature("Yandex Market")]
    class PlatformsMixSuite : BaseTestSuite
    {
        const string EXE_PATH = @"C:\Users\Vitaliy_Dobriyan\Desktop\_Release\";
        const string PORTAL_URL = @"https://market.yandex.ru/";

        [BeforeTest]
        public void BeforeTest()
        {
            Do.TimeSeriesAnalysis.StartApplication(EXE_PATH + "TimeSeriesAnalysis.exe");
        }

        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("test")]
        [Test("Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            
            Do.TimeSeriesAnalysis.OpenFile(EXE_PATH + "TestData\\henon");
            Bug("76237").YandexMarket.OpenPortal(PORTAL_URL);
            Do.YandexMarket.DoSomeActions();
        }

        [AfterTest]
        public void TearDown()
        {
            Logger.Instance.Info("After Test: close app and browser");
            Do.TimeSeriesAnalysis.CloseApplication();
            Do.YandexMarket.CloseBrowser();
        }
    }
}
