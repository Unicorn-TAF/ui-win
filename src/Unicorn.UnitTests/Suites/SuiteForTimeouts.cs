using System.Threading;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [TestSuite("Suite for timeouts")]
    [Feature("timeouts")]
    public class SuiteForTimeouts : TestSuite
    {
        [BeforeSuite]
        public void BeforeSuite()
        {
        }

        [BeforeTest]
        public void BeforeTest()
        {
        }

        [Test]
        public void Test2()
        {
            Logger.Instance.Log(LogLevel.Info, "Test2 started");
            Thread.Sleep(2100);
            Logger.Instance.Log(LogLevel.Info, "Test2 finished");
        }

        [Test]
        [Disable]
        public void TestToSkip()
        {
        }

        [Test]
        public void Test1()
        {
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
            Thread.Sleep(1900);
            Logger.Instance.Log(LogLevel.Info, "Test1 started");
        }

        [AfterTest]
        public void AfterTest()
        {
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Thread.Sleep(1000);
        }
    }
}
