using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for tests order")]
    [Tag("tests-order")]
    public class USuiteForTestsOrder : TestSuite
    {
        private string output;

        [Test]
        public void Test2() => output = "Test2>";

        [Test(3344)]
        public void Test1() => output = "Test1>";

        [Test]
        public void Test4() => output = "Test4>";

        [Test]
        public void Test3() => output = "Test3>";
    }
}
