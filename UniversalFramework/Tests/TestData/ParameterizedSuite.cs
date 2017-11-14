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
        }

        [BeforeTest]
        public void BeforeTest()
        {
            Output += "BeforeTest>";
        }

        [Test("Test 2")]
        public void Test2()
        {
            Output += "Test1>";
        }

        [Test("Test to Skip")]
        [Skip]
        public void TestToSkip()
        {
            Output += "TestToSkip>";
        }

        [Test("Test 1")]
        public void Test1()
        {
            Output += "Test2>";
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
