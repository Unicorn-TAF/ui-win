using ReportPortal.UnicornExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    class ReportPortalReporter : IReporter
    {
        ReportPortalListener Listener;
        public void Complete()
        {
            Listener.CallerRunFinished();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");
            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);

            Listener = new ReportPortalListener();
            Listener.CallerRunStarted();

            Test.onStart += this.ReportTestStart;
            Test.onFail += this.TakeScreenshot;
            Test.onFinish += this.ReportTestFinish;
            Test.onSkip += this.ReportTestSkip;

            TestSuite.onStart += this.ReportSuiteStart;
            TestSuite.onFinish += this.ReportSuiteFinish;
        }

        public void ReportInfo(string info)
        {
            Listener.CallerTestOutput(info);
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            Listener.CallerSuiteFinished(testSuite);
        }

        public void ReportSuiteStart(TestSuite testSuite)
        {
            Listener.CallerSuiteStarted(testSuite);
        }

        public void ReportTestFinish(Test test)
        {
            Listener.CallerTestFinished(test);
        }

        public void ReportTestStart(Test test)
        {
            Listener.CallerTestStarted(test);
        }

        public void ReportTestSkip(Test test)
        {
            Listener.CallerTestSkipped(test);
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
