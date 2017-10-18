using System.Threading;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    public class Suite : TestSuite
    {
        public string Output;

        [BeforeSuite]
        public void BeforeSuite()
        {
            Output = "";
            Output += "BeforeSuite>";
            Thread.Sleep(10);
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Output += "BeforeTest>";
            Thread.Sleep(10);
        }

        [Test]
        public void Test2()
        {
            Output += "Test1>";
            Thread.Sleep(10);
        }

        [Test]
        [Skip]
        public void TestToSkip()
        {
            Output += "TestToSkip>";
            Thread.Sleep(10);
        }

        [Test]
        public void Test1()
        {
            Output += "Test2>";
            Thread.Sleep(10);
            throw new System.Exception("FAILED");
        }

        [AfterTest]
        public void AfterTest()
        {
            Output += "AfterTest>";
            Thread.Sleep(10);
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Output += "AfterSuite";
            Thread.Sleep(10);
        }

    }
}
