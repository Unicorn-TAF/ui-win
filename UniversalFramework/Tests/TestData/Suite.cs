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
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Output += "BeforeTest>";
        }

        [Test]
        public void Test2()
        {
            Output += "Test1>";
        }

        [Test]
        [Skip]
        public void TestToSkip()
        {
            Output += "TestToSkip>";
        }

        [Test]
        public void Test1()
        {
            Output += "Test2>";
            throw new System.Exception("FAILED");
        }

        [AfterTest]
        public void AfterTest()
        {
            Output += "AfterTest>";
        }

        [AfterSuite]
        public void AfterSuite()
        {
            Output += "AfterSuite";
        }

    }
}
