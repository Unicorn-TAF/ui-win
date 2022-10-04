using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for timeouts 1")]
    [Tag(Tag.Timeouts)]
    public class USuiteForTimeouts1 : TestSuite
    {
        [BeforeSuite]
        public void BeforeSuite()
        {
            // Method intentionally left empty.
        }

        [BeforeTest]
        public void BeforeTest()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void Test2() =>
            Thread.Sleep(1000);

        [Test]
        [Disabled("")]
        public void TestToSkip()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void Test1() =>
            Thread.Sleep(50);

        [AfterTest]
        public void AfterTest()
        {
            // Method intentionally left empty.
        }

        [AfterSuite]
        public void AfterSuite() =>
            Thread.Sleep(250);
    }
}
