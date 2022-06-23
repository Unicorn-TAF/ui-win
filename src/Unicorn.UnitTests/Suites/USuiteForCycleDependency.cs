using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite for tests with cycle dependency")]
    [Tag(Tag.TestsCycleDependency)]
    public class USuiteForCycleDependency : TestSuite
    {
        [Test]
        [DependsOn(nameof(Test1))]
        public void Test2() => GetValue("Test2");

        [Test]
        [DependsOn(nameof(Test2))]
        public void Test1() => GetValue("Test1");

        private string GetValue(string value) => value;
    }
}
