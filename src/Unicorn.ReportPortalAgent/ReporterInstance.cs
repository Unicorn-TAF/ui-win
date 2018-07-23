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
            this.listener.ReportRunFinished();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            this.listener = new ReportPortalListener();
            this.listener.ReportRunStarted();

            Test.OnStart += ReportTestStart;
            Test.OnFail += TakeScreenshot;
            Test.OnFinish += ReportTestFinish;
            Test.OnSkip += this.listener.ReportTestSkipped;

            SuiteMethod.SuiteMethodStarted += this.listener.ReportSuiteMethodStarted;
            SuiteMethod.SuiteMethodFinished += this.listener.ReportSuiteMethodFinished;

            TestSuite.SuiteStarted += this.ReportSuiteStart;
            TestSuite.SuiteFinished += this.ReportSuiteFinish;

            TestStepsEvents.OnStart += ReportInfo;
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = TestSteps.GetStepInfo(method, arguments);
            Logger.Instance.Log(LogLevel.Info, "STEP: " + info);
            ////this.listener.ReportTestOutput(info);
        }

        public void ReportInfo(string info)
        {
            this.listener.ReportTestOutput(info);
        }

        public void ReportLoggerMessage(LogLevel level, string info)
        {
            this.listener.ReportLoggerMessage(level, info);
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            this.listener.ReportSuiteFinished(testSuite);
        }

        public void ReportSuiteStart(TestSuite testSuite)
        {
            this.listener.ReportSuiteStarted(testSuite);
        }

        public void ReportTestFinish(Test test)
        {
            this.listener.ReportTestFinished(test);
        }

        public void ReportTestStart(Test test)
        {
            this.listener.ReportTestStarted(test);
        }

        private void TakeScreenshot(Test test)
        {
            string screenshotName = test.FullName;

            if (screenshotName.Length > 150)
            {
                screenshotName = screenshotName.Substring(0, 150) + "~";
            }

            Screenshot.TakeScreenshot(screenshotName);
            test.Outcome.Screenshot = screenshotName + ".Jpeg";
        }
    }
}
