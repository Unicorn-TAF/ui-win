using System.Collections.Generic;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Parameterized test suite"), Parameterized]
    public class ParameterizedSuite : TestSuite
    {
        [SuiteData]
        public static List<TestSuiteParametersSet> GetSuiteData()
        {
            var parameters = new List<TestSuiteParametersSet>();
            parameters.Add(new TestSuiteParametersSet("set 1"));
            parameters.Add(new TestSuiteParametersSet("set 2"));
            return parameters;
        }

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
