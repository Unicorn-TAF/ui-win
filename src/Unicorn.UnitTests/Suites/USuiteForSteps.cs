using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests for reporting")]
    [Tag("Steps")]
    public class USuiteForSteps : UBaseTestSuite
    {
        [Test]
        public void Test1() =>
            Do.Testing.Say("Test1");

        [Test]
        public void Test2() =>
            Do.Testing.Say("Test2");

        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Do.Testing.Say("TestToSkip");

        [AfterTest]
        public void AfterTest() =>
            Do.Testing.Say("AfterTest");

        [AfterSuite]
        public void AfterSuite() =>
            Do.Testing.Say("AfterSuite");
    }
}
