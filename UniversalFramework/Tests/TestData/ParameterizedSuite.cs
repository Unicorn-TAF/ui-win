using System.Threading;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [ParametersSet("Set 1"), ParametersSet("Set 2")]
    public class ParameterizedSuite : TestSuite
    {
        private string output = string.Empty;

        [BeforeSuite]
        public void BeforeSuite()
        {
            output += "BeforeSuite>";
        }

        [BeforeTest]
        public void BeforeTest()
        {
            output += "BeforeTest>";
        }

        [Test("Test 2")]
        public void Test2()
        {
            output += "Test1>";
        }

        [Test("Test to Skip")]
        [Skip]
        public void TestToSkip()
        {
            output += "TestToSkip>";
        }

        [Test("Test 1")]
        public void Test1()
        {
            output += "Test2>";
        }

        [AfterTest]
        public void AfterTest()
        {
            output += "AfterTest>";
        }

        [AfterSuite]
        public void AfterSuite()
        {
            output += "AfterSuite";
        }

        public string GetOutput() => output;
    }
}
