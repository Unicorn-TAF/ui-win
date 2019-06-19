using NUnit.Framework;
using Unicorn.Taf.Core.Testing;

namespace Unicorn.UnitTests.Util
{
    public class SimpleReporter
    {
        public SimpleReporter()
        {
            Test.OnTestStart += this.ReportTestStart;
            Test.OnTestFinish += this.ReportTestFinish;
        }

        public void ReportInfo(string info) =>
            TestContext.WriteLine($"REPORTER: {info}");

        public void ReportSuiteFinish(TestSuite testSuite) =>
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Outcome.Name}' {testSuite.Outcome.Result}");

        public void ReportTestStart(Test test) =>
            TestContext.WriteLine($"REPORTER: Test '{test.Outcome.Title}' started");

        public void ReportTestFinish(Test test) =>
            TestContext.WriteLine($"REPORTER: Test '{test.Outcome.Title}' {test.Outcome.Result}");

        public void ReportSuiteStart(TestSuite testSuite) =>
            TestContext.WriteLine($"REPORTER: Suite '{testSuite.Outcome.Name}' started");
    }
}
