using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for tests order attribute")]
    [Tag("tests-order-attribute")]
    public class USuiteForTestsOrderAttribute : TestSuite
    {
        [Test]
        public void Test7() => GetValue("Test5");

        [Test]
        public void Test2() => GetValue("Test2");

        [Test]
        public void Test4() => GetValue("Test4");

        [Test]
        [Order(2)]
        public void Test3() => GetValue("Test3");

        [Test]
        public void Test6() => GetValue("Test6");

        [Test]
        [Order(51)]
        public void Test5() => GetValue("Test5");

        [Test]
        public void Test1() => GetValue("Test1");

        private string GetValue(string value) => value;
    }
}
