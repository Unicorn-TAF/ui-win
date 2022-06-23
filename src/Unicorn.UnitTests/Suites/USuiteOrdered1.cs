using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Ordered suite 1")]
    [Tag(Tag.Ordering)]
    public class USuiteOrdered1 : TestSuite
    {
        public string Output { get; set; }

        [Test("Test1-1")]
        [Category("category3")]
        public void Test1() =>
            Output += "Test1>";

        [Test("Test1-2")]
        [Disabled("")]
        [Category("category2")]
        public void Test2() =>
            Output += "Test2>";

        [Test("Test1-3")]
        [Category("category1")]
        public void Test3()
        {
            Output += "Test3>";
        }
    }
}
