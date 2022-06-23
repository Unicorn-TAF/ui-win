using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests (all skipped)")]
    [Tag(Tag.Skipping)]
    public class USuiteToBeSkipped : UBaseTestSuite
    {
        public static string Output { get; set; }

        [BeforeSuite]
        public void BeforeSuite() =>
            Output += "BeforeSuite";

        [BeforeTest]
        public void BeforeTest() =>
            Output += "BeforeTest";

        [Test]
        [Category("someCategory"), Category("thirdCategory")]
        public void Test2()
        {
            Output += "Test2";
            Do.Assertion.StartAssertionsChain(null);
        }

        [Disabled("")]
        [Test]
        public void TestToSkip()
        {
            Output += "TestToSkip";
            Do.Assertion.StartAssertionsChain("a");
        }

        [Test]
        [Category("someCategory"), Category("anotherCategory")]
        public void Test1()
        {
            Output += "Test1";
            Bug("871236").Assertion.AssertThat(new SampleObject(), Is.Null());
        }

        [Disabled("")]
        [Test]
        public void Test23()
        {
            Output += "Test23";
            Bug("871236").Assertion.StartAssertionsChain("a");
        }

        [Test]
        [Category("someCategory")]
        public void Test33()
        {
            Output += "Test33";
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());
        }

        [Disabled("")]
        [Test]
        [Category("someCategory")]
        public void Test43()
        {
            Output += "Test43";
            Bug("871236").Assertion.StartAssertionsChain("a");
            Do.Assertion.AssertThat(new SampleObject(), Is.Null());
        }

        [AfterTest]
        public void AfterTest() =>
            Output += "AfterTest";

        [AfterSuite]
        public void AfterSuite() =>
            Output += "AfterSuite";
    }
}
