using NUnit.Framework;
using System.IO;
using System.Reflection;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;

namespace ProjectSpecific.Util
{
    public class SimpleReporter : IReporter
    {
        public void Complete()
        {
            ////throw new NotImplementedException();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");

            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            Test.OnStart += this.ReportTestStart;
            Test.OnFinish += this.ReportTestFinish;
            Test.OnFail += this.TakeScreenshot;
        }

        public void ReportInfo(string info)
        {
            TestContext.WriteLine($"REPORTER: {info}");
        }

        public void ReportSuiteFinish(TestSuite testSuite)
        {
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Name}' {testSuite.Outcome.Result}");
        }

        public void ReportTestStart(Test test)
        {
            TestContext.WriteLine($"REPORTER: Test '{test.Description}' started");
        }

        public void ReportTestFinish(Test test)
        {
            TestContext.WriteLine($"REPORTER: Test '{test.Description}' {test.Outcome.Result}");
        }

        public void ReportSuiteStart(TestSuite testSuite)
        {
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Name}' started");
        }

        private void TakeScreenshot(Test test)
        {
            Screenshot.TakeScreenshot(test.FullTestName);
            test.Outcome.Screenshot = test.FullTestName + ".Jpeg";
        }
    }
}
