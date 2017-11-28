using ReportPortal.UnicornExtension;
using System.IO;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    class ReportPortalReporter : IReporter
    {

        ReportPortalListener Listener;
        public void Complete()
        {
            Listener.ReportRunFinished();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");
            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);

            Listener = new ReportPortalListener();
            //Listener.ExistingLaunchId = Listener.GetLaunchId("Unit tests of Unicorn Framework");
            Listener.ReportRunStarted();
            

            Test.onStart += ReportTestStart;
            Test.onFail += TakeScreenshot;
            Test.onFinish += ReportTestFinish;
            Test.onSkip += Listener.ReportTestSkipped;

            TestSuiteMethod.onStart += Listener.ReportSuiteMethodStarted;
            TestSuiteMethod.onFinish += Listener.ReportSuiteMethodFinished;

            TestSuite.onStart += this.ReportSuiteStart;
            TestSuite.onFinish += this.ReportSuiteFinish;

            TestStepsEvents.onStart += ReportInfo;
        }

        public void ReportInfo(MethodBase method, object[] arguments)
        {
            string info = TestSteps.GetStepInfo(method, arguments);
            Logger.Instance.Info("STEP: " + info);
            Listener.ReportTestOutput(info);
        }

        public void ReportInfo(string info)
        {
            Listener.ReportTestOutput(info);
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            Listener.ReportSuiteFinished(testSuite);
        }

        public void ReportSuiteStart(TestSuite testSuite)
        {
            Listener.ReportSuiteStarted(testSuite);
        }

        public void ReportTestFinish(Test test)
        {
            Listener.ReportTestFinished(test);
        }

        public void ReportTestStart(Test test)
        {
            Listener.ReportTestStarted(test);
        }


        private void TakeScreenshot(Test test)
        {
            string screenshotName = test.FullTestName;
            if (screenshotName.Length > 150)
                screenshotName = screenshotName.Substring(0, 150) + "~";

            Screenshot.TakeScreenshot(screenshotName);
            test.Outcome.Screenshot = screenshotName + ".Jpeg";
        }
    }
}
