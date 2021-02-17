using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite")]
    [Tag("sample")]
    public class USuite : TestSuite
    {
        public static string Output { get; set; }

        [BeforeSuite]
        public void BeforeSuite() =>
            Output += "BeforeSuite>";

        [BeforeTest]
        public void BeforeTest() =>
            Output += "BeforeTest>";

        [Author("Author2")]
        [Test]
        public void Test2() =>
            Output += "Test1>";

        [Test]
        [Disabled("")]
        public void TestToSkip() =>
            Output += "TestToSkip>";

        [Test]
        public void Test1()
        {
            Output += "Test2>";
            throw new System.Exception("FAILED");
        }

        [AfterTest]
        public void AfterTest() =>
            Output += "AfterTest>";

        [AfterSuite]
        public void AfterSuite() =>
            Output += "AfterSuite";
    }
}
