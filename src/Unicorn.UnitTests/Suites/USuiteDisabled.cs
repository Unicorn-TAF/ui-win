using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Disabled("By design")]
    [Suite("Suite disabled")]
    [Tag(Tag.Disabled)]
    public class USuiteDisabled : UBaseTestSuite
    {
        public static string Output { get; set; }

        [Test]
        [Category("disabledSuiteCategory")]
        public void Test2()
        {
            Output += "Test2";
            Do.Assertion.StartAssertionsChain(null);
        }

        [Test]
        [Category("disabledSuiteCategory")]
        public void Test1()
        {
            Output += "Test1";
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());
        }

        [Test]
        public void Test23()
        {
            Output += "Test23";
            Do.Assertion.StartAssertionsChain("a");
        }
    }
}
