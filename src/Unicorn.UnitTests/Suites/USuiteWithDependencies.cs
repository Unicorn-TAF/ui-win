using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite with dependencies")]
    [Tag(Tag.Dependencies)]
    public class USuiteWithDependencies : TestSuite
    {
        public static string Output { get; set; }

        [Test("Test 1")]
        public void Test1() =>
            Output += "Test1>";

        [Test("Test 2")]
        public void Test2() =>
            Output += "Test2>";

        [Test("Test with fail")]
        public void TestWithFail()
        {
            throw new System.Exception("FAILED");
        }

        [Test("Test 3")]
        [DependsOn("TestWithFail")]
        public void Test3() =>
            Output += "Test3>";

        [Test("Failed test depending on failed test")]
        [DependsOn("TestWithFail")]
        public void FailedTestDependingOnFailedTtest()
        {
            throw new System.Exception("FAILED");
        }

        [Test("Test 4")]
        [DependsOn("Test2")]
        public void Test4() =>
            Output += "Test4>";
    }
}
