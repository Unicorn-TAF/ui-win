using System.Threading;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [ParametersSet("Set 1"), ParametersSet("Set 2")]
    public class ParameterizedSuite : TestSuite
    {
        public string Output = "";

        [BeforeSuite]
        public void BeforeSuite()
        {
            Output += "BeforeSuite>";
            Thread.Sleep(10);
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Output += "BeforeTest>";
            Thread.Sleep(10);
        }

        [Test("Test 2")]
        public void Test2()
        {
            Output += "Test1>";
            Thread.Sleep(10);
        }

        [Test("Test to Skip")]
        [Skip]
        public void TestToSkip()
        {
            Output += "TestToSkip>";
            Thread.Sleep(10);
        }

        [Test("Test 1")]
        public void Test1()
        {
            Output += "Test2>";
            Thread.Sleep(10);
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
