using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
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
            Do.Assertion.StartAssertionsChain(null);

        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Do.Assertion.StartAssertionsChain("a");

        [Test]
        public void Test1() =>
            Bug("871236").Assertion.AssertThat(new SampleObject(), Is.Null());

        [Test]
        public void Test23() =>
            Bug("2343").Assertion.StartAssertionsChain("a");

        [Test]
        public void Test53() =>
            Bug("234").Assertion.AssertThat(new SampleObject(), Is.Null());

        [Test]
        public void Test33() =>
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());

        [Test]
        public void Test43()
        {
            Bug("8716").Assertion.StartAssertionsChain("a");
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());
        }

        [AfterTest]
        public void AfterTest() =>
            Thread.Sleep(1);

        [AfterSuite]
        public void AfterSuite() =>
            Thread.Sleep(1);
    }
}
