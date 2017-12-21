using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Suite")]
    public class Suite : TestSuite
    {
        private string output;

        [BeforeSuite]
        public void BeforeSuite()
        {
            output = string.Empty;
            output += "BeforeSuite>";
        }

        [BeforeTest]
        public void BeforeTest()
        {
            output += "BeforeTest>";
        }

        [Test]
        public void Test2()
        {
            output += "Test1>";
        }

        [Test]
        [Skip]
        public void TestToSkip()
        {
            output += "TestToSkip>";
        }

        [Test]
        public void Test1()
        {
            output += "Test2>";
            throw new System.Exception("FAILED");
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
