using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Reporting;
using Unicorn.Taf.Core.Testing.Steps;
using Unicorn.Taf.Core.Testing.Tests;
using Unicorn.Taf.Core.Utility;

namespace Unicorn.ReportPortalAgent
{
    public class ReporterInstance : IReporter
    {
        private ReportPortalListener listener;

        public void Complete()
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                this.listener.FinishRun();
            }
        }

        public void Init()
        {
            if (ReportPortalListener.Config.IsEnabled)
            {
                if (!Directory.Exists(Screenshotter.ScreenshotsFolder))
                {
                    Directory.CreateDirectory(Screenshotter.ScreenshotsFolder);
                }

                this.listener = new ReportPortalListener();
                this.listener.StartRun();

                Test.OnTestStart += ReportTestStart;
                Test.OnTestFail += TakeScreenshot;
                Test.OnTestFinish += ReportTestFinish;
                Test.OnTestSkip += this.listener.ReportTestSkipped;

                SuiteMethod.OnSuiteMethodStart += this.listener.StartSuiteMethod;
                SuiteMethod.OnSuiteMethodFail += TakeScreenshot;
                SuiteMethod.OnSuiteMethodFinish += this.listener.FinishSuiteMethod;

                TestSuite.OnSuiteStart += this.ReportSuiteStart;
                TestSuite.OnSuiteFinish += this.ReportSuiteFinish;

                StepsEvents.OnStepStart += ReportInfo;
            }
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = StepsUtilities.GetStepInfo(method, arguments);
            this.ReportInfo(info);
            Logger.Instance.Log(LogLevel.Info, "STEP: " + info);
        }

        public void ReportInfo(string info) =>
            this.listener.ReportTestMessage(LogLevel.Info, info);

        public void ReportSuiteFinish(TestSuite testSuite) =>
            this.listener.FinishSuite(testSuite);

        public void ReportSuiteStart(TestSuite testSuite) =>
            this.listener.StartSuite(testSuite);

        public void ReportTestFinish(Test test) =>
            this.listener.FinishSuiteMethod(test);

        public void ReportTestStart(Test test) =>
            this.listener.StartSuiteMethod(test);

        public void TakeScreenshot(SuiteMethod suiteMethod) =>
            suiteMethod.Outcome.Screenshot = Screenshotter.TakeScreenshot(suiteMethod.FullName);
    }
}
