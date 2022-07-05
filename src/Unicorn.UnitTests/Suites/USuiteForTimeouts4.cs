using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for timeouts 4")]
    [Tag(Tag.SuiteTimeouts)]
    public class USuiteForTimeouts4 : TestSuite
    {
        internal const int Timeout = 250;

        [BeforeSuite]
        public void BeforeSuite() => Thread.Sleep(Timeout);

        [BeforeTest]
        public void BeforeTest() => Thread.Sleep(Timeout);

        [Test]
        public void Test1() => Thread.Sleep(Timeout);

        [Test]
        public void Test2() => Thread.Sleep(Timeout);

        [Test, Disabled("")]
        public void TestToSkip()
        {
            // Method intentionally left empty.
        }

        [AfterTest]
        public void AfterTest() => Thread.Sleep(Timeout);
    }
}
