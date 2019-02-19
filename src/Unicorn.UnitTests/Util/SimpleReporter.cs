using System.IO;
using NUnit.Framework;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

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
            if (!Directory.Exists(Screenshot.ScreenshotsFolder))
            {
                Directory.CreateDirectory(Screenshot.ScreenshotsFolder);
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
            TestContext.WriteLine($"REPORTER: Test '{test.Description}' started");

        public void ReportTestFinish(Test test) =>
            TestContext.WriteLine($"REPORTER: Test '{test.Description}' {test.Outcome.Result}");

        public void ReportSuiteStart(TestSuite testSuite) =>
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Name}' started");

        private void TakeScreenshot(Test test) =>
            test.Outcome.Screenshot = Screenshot.TakeScreenshot(test.FullName);
    }
}
