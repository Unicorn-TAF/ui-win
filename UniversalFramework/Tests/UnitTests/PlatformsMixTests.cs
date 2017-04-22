using NUnit.Framework;
using ProjectSpecific.Steps;

namespace Tests.UnitTests
{

    public class PlatformsMixTests
    {
        TimeSeriesAnalysisSteps TSAnalysis = new TimeSeriesAnalysisSteps();
        WebSteps Web = new WebSteps();

        const string EXE_PATH = @"D:\SCIENCE\Programs\MathAnalysisSoftware\_Release\";
        const string PORTAL_URL = @"https://devmi3-clients.ileveldev.com/";


        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Run actions across different platforms using common IDriver instance")]
        public void SingleDriverTest()
        {
            TSAnalysis.StartApplication(EXE_PATH + "TimeSeriesAnalysis.exe");
            TSAnalysis.OpenFile(EXE_PATH + "TestData\\henon");
            Web.OpenPortal(PORTAL_URL);
            Web.DoSomeActions();
            Web.CloseBrowser();
        }
    }
}
