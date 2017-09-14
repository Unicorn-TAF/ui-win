using ProjectSpecific.BO;
using System.Threading;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Tests for reporting")]
    [Feature("Reporting")]
    class SuiteForReporting : BaseTestSuite
    {
        [BeforeSuite]
        public void BeforeSuite()
        {
            Thread.Sleep(10);
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Thread.Sleep(10);
        }

        [Test]
        public void Test2()
        {
            Do.Testing.FirstTestStep();
        }

        [Test]
        [Skip]
        public void TestToSkip()
        {
            Do.Testing.SecondTestStep("a");
        }

        [Test]
        public void Test1()
        {
            Bug("871236").Testing.StepWhichSouldFail(new SampleObject());
        }

        [Test]
        public void Test23()
        {
            Bug("871236").Testing.SecondTestStep("a");
        }

        [Test]
        public void Test33()
        {
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [Test]
        public void Test43()
        {
            Bug("871236").Testing.SecondTestStep("a");
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [AfterTest]
        public void AfterTest()
        {
            Thread.Sleep(10);
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Thread.Sleep(10);
        }
    }
}
