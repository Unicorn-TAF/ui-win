using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Tests (all skipped)")]
    [Tag("skipping")]
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
            Do.Testing.FirstTestStep();
        }

        [Disabled("")]
        [Test]
        public void TestToSkip()
        {
            Output += "TestToSkip";
            Do.Testing.Say("a");
        }

        [Test]
        [Category("someCategory"), Category("anotherCategory")]
        public void Test1()
        {
            Output += "Test1";
            Bug("871236").Testing.StepWhichSouldFail(new SampleObject());
        }

        [Disabled("")]
        [Test]
        public void Test23()
        {
            Output += "Test23";
            Bug("871236").Testing.Say("a");
        }

        [Test]
        [Category("someCategory")]
        public void Test33()
        {
            Output += "Test33";
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [Disabled("")]
        [Test]
        [Category("someCategory")]
        public void Test43()
        {
            Output += "Test43";
            Bug("871236").Testing.Say("a");
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [AfterTest]
        public void AfterTest() =>
            Output += "AfterTest";

        [AfterSuite]
        public void AfterSuite() =>
            Output += "AfterSuite";
    }
}
