using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests for reporting")]
    [Tag(Tag.Steps)]
    public class USuiteForSteps : UBaseTestSuite
    {
        [Test]
        public void Test1() =>
            Do.Assertion.StartAssertionsChain("Test1");

        [Test]
        public void Test2() =>
            Do.Assertion.AssertThat("Test2", Is.EqualTo("Test2"));

        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Do.Assertion.StartAssertionsChain("TestToSkip");

        [AfterTest]
        public void AfterTest() =>
            Do.Assertion.StartAssertionsChain("AfterTest");

        [AfterSuite]
        public void AfterSuite() =>
            Do.Assertion.StartAssertionsChain("AfterSuite");
    }
}
