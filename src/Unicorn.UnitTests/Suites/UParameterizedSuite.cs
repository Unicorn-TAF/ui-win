using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Parameterized test suite"), Parameterized]
    [Tag(Tag.Parameterized)]
    public class UParameterizedSuite : TestSuite
    {
        private readonly SampleObject so;

        public UParameterizedSuite()
        {
        }

        public UParameterizedSuite(SampleObject so)
        {
            this.so = so;
        }

        public static string Output { get; set; }

        [SuiteData]
        public static List<DataSet> GetSuiteData()
        {
            var parameters = new List<DataSet>();
            parameters.Add(new DataSet("set 1", new SampleObject("a", 2)));
            parameters.Add(new DataSet("set 2", new SampleObject("b", 3)));
            return parameters;
        }

        [BeforeSuite]
        public void BeforeSuite()
        {
            Output += so.ToString();
            Output += ">BeforeSuite>";
        }

        [BeforeTest]
        public void BeforeTest() =>
            Output += "BeforeTest>";

        [Test("Test 2")]
        public void Test2() =>
            Output += "Test1>";

        [Test("Test to Skip")]
        [Disabled("")]
        public void TestToSkip() =>
            Output += "TestToSkip>";

        [Test("Test 1")]
        public void Test1() =>
            Output += "Test2>";

        [AfterTest]
        public void AfterTest() =>
            Output += "AfterTest>";

        [AfterSuite]
        public void AfterSuite() =>
            Output += "AfterSuite";
    }
}
