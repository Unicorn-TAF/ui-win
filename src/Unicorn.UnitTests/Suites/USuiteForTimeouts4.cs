using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for timeouts 4")]
    [Tag("suite-timeouts")]
    public class USuiteForTimeouts4 : TestSuite
    {
        [BeforeSuite]
        public void BeforeSuite() => Thread.Sleep(1500);

        [BeforeTest]
        public void BeforeTest() => Thread.Sleep(1500);

        [Test]
        public void Test1() => Thread.Sleep(1500);

        [Test]
        public void Test2() => Thread.Sleep(1500);

        [Test, Disabled("")]
        public void TestToSkip()
        {
            // Method intentionally left empty.
        }

        [AfterTest]
        public void AfterTest() => Thread.Sleep(1500);
    }
}
