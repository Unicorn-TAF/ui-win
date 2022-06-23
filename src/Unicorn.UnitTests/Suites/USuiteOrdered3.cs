using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Ordered suite 3")]
    [Tag(Tag.Ordering)]
    public class USuiteOrdered3 : TestSuite
    {
        public string Output { get; set; }

        [Test("Test3-1")]
        [Category("category3")]
        public void Test1() =>
            Output += "Test1>";

        [Test("TestToSkip")]
        [Disabled("")]
        [Category("category1")]
        public void TestToSkip() =>
            Output += "TestToSkip>";

        [Test("Test3-3")]
        [Category("category2")]
        public void Test2()
        {
            Output += "Test2>";
            throw new System.Exception("FAILED");
        }
    }
}
