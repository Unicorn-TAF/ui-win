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
        Steps Do;

        const string EXE_PATH = @"C:\Users\Vitaliy_Dobriyan\Desktop\_Release\";
        const string PORTAL_URL = @"https://market.yandex.ru/";

        [BeforeTest]
        public void BeforeTest()
        {
            Do = new Steps();
        }

        //[Author("Vitaliy Dobriyan")]
        [Test("Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            Do.TimeSeriesAnalysis.StartApplication(EXE_PATH + "TimeSeriesAnalysis.exe");
            Do.TimeSeriesAnalysis.OpenFile(EXE_PATH + "TestData\\henon");
            Do.YandexMarket.OpenPortal(PORTAL_URL);
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
