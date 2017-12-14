using System.IO;
using System.Reflection;
using ReportPortal.UnicornExtension;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    public class ReportPortalReporter : IReporter
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
            ////Listener.ExistingLaunchId = Listener.GetLaunchId("Unit tests of Unicorn Framework");
            this.listener.ReportRunStarted();

            Test.OnStart += ReportTestStart;
            Test.OnFail += TakeScreenshot;
            Test.OnFinish += ReportTestFinish;
            Test.OnSkip += this.listener.ReportTestSkipped;

            TestSuiteMethod.OnStart += this.listener.ReportSuiteMethodStarted;
            TestSuiteMethod.OnFinish += this.listener.ReportSuiteMethodFinished;

            TestSuite.OnStart += this.ReportSuiteStart;
            TestSuite.OnFinish += this.ReportSuiteFinish;

            TestStepsEvents.OnStart += ReportInfo;
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = TestSteps.GetStepInfo(method, arguments);
            Logger.Instance.Info("STEP: " + info);
            this.listener.ReportTestOutput(info);
        }

        public void ReportInfo(string info)
        {
            this.listener.ReportTestOutput(info);
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
            string screenshotName = test.FullTestName;

            if (screenshotName.Length > 150)
            {
                screenshotName = screenshotName.Substring(0, 150) + "~";
            }

            Screenshot.TakeScreenshot(screenshotName);
            test.Outcome.Screenshot = screenshotName + ".Jpeg";
        }
    }
}
