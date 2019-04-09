using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests for reporting")]
    [Tag("Reporting")]
    public class USuiteForReporting : UBaseTestSuite
    {
        [BeforeSuite]
        public void BeforeSuite() =>
            Thread.Sleep(1);

        [BeforeTest]
        public void BeforeTest() =>
            Thread.Sleep(1);

        [Test]
        public void Test2() =>
            Do.Testing.FirstTestStep();

        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Do.Testing.Say("a");

        [Test]
        public void Test1() =>
            Bug("871236").Testing.StepWhichSouldFail(new SampleObject());

        [Test]
        public void Test23() =>
            Bug("2343").Testing.Say("a");

        [Test]
        public void Test53() =>
            Bug("234").Testing.StepWhichSouldFail(new SampleObject());

        [Test]
        public void Test33() =>
            Do.Testing.StepWhichSouldFail(new SampleObject());

        [Test]
        public void Test43()
        {
            Bug("8716").Testing.Say("a");
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [AfterTest]
        public void AfterTest() =>
            Thread.Sleep(1);

        [AfterSuite]
        public void AfterSuite() =>
            Thread.Sleep(1);
    }
}
