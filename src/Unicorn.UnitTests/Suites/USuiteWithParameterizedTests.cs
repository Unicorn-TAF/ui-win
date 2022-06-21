using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite with parameterized tests")]
    [Tag(Tag.ParameterizedTests)]
    public class USuiteWithParameterizedTests : TestSuite
    {
        public static string Output { get; set; }
        
        public static List<DataSet> GetTestData()
        {
            var parameters = new List<DataSet>();
            parameters.Add(new DataSet("set 2", new SampleObject("b", 3)));
            parameters.Add(new DataSet("set 1", new SampleObject("a", 2)));
            return parameters;
        }

        [BeforeSuite]
        public void BeforeSuite() =>
            Output += "BeforeSuite>";

        [BeforeTest]
        public void BeforeTest() =>
            Output += "BeforeTest>";

        [Test("Test 1"), TestData("GetTestData")]
        public void Test1(SampleObject so)
        {
            Output += so.ToString();
            Output += "Test1>";
        }

        [Test("Test 2")]
        public void Test2() =>
            Output += "Test2>";

        [Test("Test to Skip")]
        [Disabled("")]
        public void TestToSkip() =>
            Output += "TestToSkip>";

        [AfterTest]
        public void AfterTest() =>
            Output += "AfterTest>";

        [AfterSuite]
        public void AfterSuite() =>
            Output += "AfterSuite";
    }
}
