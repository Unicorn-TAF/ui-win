using System.IO;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Tests;

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
                if (!Directory.Exists(Screenshot.ScreenshotsFolder))
                {
                    Directory.CreateDirectory(Screenshot.ScreenshotsFolder);
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

                TestStepsEvents.OnStepStart += ReportInfo;
            }
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = TestSteps.GetStepInfo(method, arguments);
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
            suiteMethod.Outcome.Screenshot = Screenshot.TakeScreenshot(suiteMethod.FullName);
    }
}
