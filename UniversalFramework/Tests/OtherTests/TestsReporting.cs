using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.BO;
using ProjectSpecific.Steps;
using System;
using Tests.TestData;

namespace Tests.UnitTests
{
    [TestFixture]
    class TestReportingParameterizedTestSuite : NUnitReportPortalTestRunner
    {
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void ParameterizedSuiteRunSuiteTest()
        {
            ParameterizedSuite suite = Activator.CreateInstance<ParameterizedSuite>();
            suite.Run();
        }


        Steps Do = new Steps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void StepsReportingTest()
        {
            Do.Testing.ThirdTestStep(3);
            Do.Testing.FourthTestStep(new SampleObject());
            Do.Testing.FirstTestStep();
            Do.Testing.SecondTestStep("value");
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
