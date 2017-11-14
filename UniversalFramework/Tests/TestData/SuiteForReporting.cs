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
            Thread.Sleep(1);
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Thread.Sleep(1);
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
            Bug("2343").Testing.SecondTestStep("a");
        }

        [Test]
        public void Test53()
        {
            Bug("234").Testing.StepWhichSouldFail(new SampleObject());
        }

        [Test]
        public void Test33()
        {
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [Test]
        public void Test43()
        {
            Bug("8716").Testing.SecondTestStep("a");
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [AfterTest]
        public void AfterTest()
        {
            Thread.Sleep(1);
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Thread.Sleep(1);
        }
    }
}
