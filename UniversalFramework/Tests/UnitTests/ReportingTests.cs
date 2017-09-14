using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.Steps;
using ProjectSpecific.BO;
using Tests.TestData;

namespace Tests.UnitTests
{
    class ReportingTests : NUnitTestRunner
    {
        Steps Do = new Steps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void StepsReportingTest()
        {
            string checkString = "STEP: Third Test Step '3'\r\n\r\nSTEP: Fourth Test Step 'complex object with param a = 12'\r\n\r\nSTEP: First Test Step\r\n\r\nSTEP: Second Test Step 'value'\r\n\r\n";
            Do.Testing.ThirdTestStep(3);
            Do.Testing.FourthTestStep(new SampleObject());
            Do.Testing.FirstTestStep();
            Do.Testing.SecondTestStep("value");
            Assert.That(GetTestContextOut(), Is.EqualTo(checkString));
        }


        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging 2")]
        public void StepsReportingTest2()
        {
            SuiteForReporting suite = new SuiteForReporting();
            suite.Run();
        }
    }
}
