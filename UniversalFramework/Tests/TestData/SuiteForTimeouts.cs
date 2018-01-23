using System.Threading;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
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
            Unicorn.Core.Logging.Logger.Instance.Info("Test2 started");
            Thread.Sleep(2100);
            Unicorn.Core.Logging.Logger.Instance.Info("Test2 finished");
        }

        [Test]
        [Disable]
        public void TestToSkip()
        {
        }

        [Test]
        public void Test1()
        {
            Unicorn.Core.Logging.Logger.Instance.Info("Test1 started");
            Thread.Sleep(1900);
            Unicorn.Core.Logging.Logger.Instance.Info("Test1 started");
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
