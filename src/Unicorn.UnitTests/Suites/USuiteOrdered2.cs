using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Ordered suite 2")]
    [Tag(Tag.Ordering)]
    public class USuiteOrdered2 : TestSuite
    {
        public string Output { get; set; }

        [Test("Test2-1")]
        [Category("category2")]
        public void Test1() =>
            Output += "Test1>";

        [Test("Test2-2")]
        [Category("category1")]
        public void Test2() =>
            Output += "Test2>";

        [Test("Test2-3")]
        [Category("category2")]
        public void Test3()
        {
            throw new System.Exception("FAILED");
        }
    }
}
