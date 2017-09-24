using System;
using System.IO;
using System.Reflection;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Tests;
using NUnit.Framework;

namespace ProjectSpecific.Util
{
    public class SimpleReporter : IReporter
    {
        public void Complete()
        {
            //throw new NotImplementedException();
        }

        public void Init()
        {
            string screenshotsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Screenshots");

            if (!Directory.Exists(screenshotsDir))
                Directory.CreateDirectory(screenshotsDir);

            Test.onStart += this.ReportTestStart;
            Test.onFinish += this.ReportTestFinish;
            Test.onFail += this.TakeScreenshot;
            //throw new NotImplementedException();
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

        private void TakeScreenshot(Test test)
        {
            Screenshot.TakeScreenshot(test.FullTestName);
            test.Outcome.Screenshot = test.FullTestName + ".Jpeg";
        }
    }
}
