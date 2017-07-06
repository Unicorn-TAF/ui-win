using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.Steps;

namespace Tests.UnitTests
{

    public class PlatformsMixTests : NUnitTestRunner
    {
        TimeSeriesAnalysisSteps TSAnalysis = new TimeSeriesAnalysisSteps();
        WebSteps Web = new WebSteps();

        const string EXE_PATH = @"C:\Users\Vitaliy_Dobriyan\Desktop\_Release\";
        const string PORTAL_URL = @"https://devmi3-clients.ileveldev.com/";


        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            TSAnalysis.StartApplication(EXE_PATH + "TimeSeriesAnalysis.exe");
            TSAnalysis.OpenFile(EXE_PATH + "TestData\\henon");
            Web.OpenPortal(PORTAL_URL);
            Web.DoSomeActions();
        }

        [TearDown]
        public void TearDown()
        {
            TSAnalysis.CloseApplication();
            Web.CloseBrowser();
        }
    }
}
