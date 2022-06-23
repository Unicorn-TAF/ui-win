using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Parameterized test suite with no data"), Parameterized]
    [Tag(Tag.ParameterizedBroken)]
    public class UParameterizedSuiteNoSuiteData : TestSuite
    {
        public UParameterizedSuiteNoSuiteData(SampleObject so)
        {
        }

        public static string Output { get; set; }

        [Test("Test 1")]
        public void Test1() =>
            Output += "Test1>";
    }
}
