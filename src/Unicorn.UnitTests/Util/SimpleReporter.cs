using System.IO;
using NUnit.Framework;
using Unicorn.Taf.Core.Reporting;
using Unicorn.Taf.Core.Testing.Tests;
using Unicorn.Taf.Core.Utility;

namespace Unicorn.UnitTests.Util
{
    public class SimpleReporter : IReporter
    {
        public void Complete()
        {
            ////throw new NotImplementedException();
        }

        public void Init()
        {
            if (!Directory.Exists(Screenshotter.ScreenshotsFolder))
            {
                Directory.CreateDirectory(Screenshotter.ScreenshotsFolder);
            }

            Test.OnTestStart += this.ReportTestStart;
            Test.OnTestFinish += this.ReportTestFinish;
            Test.OnTestFail += this.TakeScreenshot;
        }

        public void ReportInfo(string info) =>
            TestContext.WriteLine($"REPORTER: {info}");

        public void ReportSuiteFinish(TestSuite testSuite) =>
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Name}' {testSuite.Outcome.Result}");

        public void ReportTestStart(Test test) =>
            TestContext.WriteLine($"REPORTER: Test '{test.Outcome.Title}' started");

        public void ReportTestFinish(Test test) =>
            TestContext.WriteLine($"REPORTER: Test '{test.Outcome.Title}' {test.Outcome.Result}");

        public void ReportSuiteStart(TestSuite testSuite) =>
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Name}' started");

        private void TakeScreenshot(Test test) =>
            test.Outcome.Screenshot = Screenshotter.TakeScreenshot(test.Outcome.FullMethodName);
    }
}
