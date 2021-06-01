using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for tests order")]
    [Tag("tests-order")]
    public class USuiteForTestsOrder : TestSuite
    {
        [Test]
        public void Test2() => GetValue("Test2");

        [Test(3344)]
        public void Test1() => GetValue("Test1");

        [Test]
        public void Test4() => GetValue("Test4");

        [Test]
        public void Test3() => GetValue("Test3");

        [Test]
        public void Test6() => GetValue("Test6");

        [Test]
        public void Test5() => GetValue("Test5");

        private string GetValue(string value) => value;
    }
}
