using System.Reflection;
using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.BO;
using ProjectSpecific.Steps;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestReportingParameterizedTestSuite : NUnitReportPortalTestRunner
    {
        protected Steps Do => new Steps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Check suite run")]
        public void ParameterizedSuiteRunSuiteTest()
        {
            Unicorn.Core.Testing.Tests.Adapter.Configuration.SetSuiteFeatures("parameterized");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
        }

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
            Unicorn.Core.Testing.Tests.Adapter.Configuration.SetSuiteFeatures("reporting");
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly(), false);
            runner.RunTests();
        }
    }
}
