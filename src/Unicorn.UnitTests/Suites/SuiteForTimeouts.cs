using System.Threading;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [TestSuite("Suite for timeouts")]
    [Tag("timeouts")]
    public class SuiteForTimeouts : TestSuite
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
        public void Test2()
        {
            Logger.Instance.Log(LogLevel.Info, "Test2 started");
            Thread.Sleep(1400);
            Logger.Instance.Log(LogLevel.Info, "Test2 finished");
        }

        [Test]
        [Disable]
        public void TestToSkip()
        {
            // Method intentionally left empty.
        }

        [Test]
        public void Test1()
        {
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
            Thread.Sleep(900);
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
        }

        [AfterTest]
        public void AfterTest()
        {
            // Method intentionally left empty.
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Thread.Sleep(1000);
        }
    }
}
