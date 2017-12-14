using ProjectSpecific.BO;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    [TestSuite("Tests (all skipped)")]
    [Feature("Skipping")]
    public class SuiteToBeSkipped : BaseTestSuite
    {
        private string output = string.Empty;

        [BeforeSuite]
        public void BeforeSuite()
        {
            output += "BeforeSuite";
        }

        [BeforeTest]
        public void BeforeTest()
        {
            output += "BeforeTest";
        }

        [Test]
        [Category("someCategory"), Category("thirdCategory")]
        public void Test2()
        {
            output += "Test2";
            Do.Testing.FirstTestStep();
        }

        [Skip]
        [Test]
        public void TestToSkip()
        {
            output += "TestToSkip";
            Do.Testing.SecondTestStep("a");
        }

        [Test]
        [Category("someCategory"), Category("anotherCategory")]
        public void Test1()
        {
            output += "Test1";
            Bug("871236").Testing.StepWhichSouldFail(new SampleObject());
        }

        [Skip]
        [Test]
        public void Test23()
        {
            output += "Test23";
            Bug("871236").Testing.SecondTestStep("a");
        }

        [Test]
        [Category("someCategory")]
        public void Test33()
        {
            output += "Test33";
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [Skip]
        [Test]
        [Category("someCategory")]
        public void Test43()
        {
            output += "Test43";
            Bug("871236").Testing.SecondTestStep("a");
            Do.Testing.StepWhichSouldFail(new SampleObject());
        }

        [AfterTest]
        public void AfterTest()
        {
            output += "AfterTest";
        }

        [AfterSuite]
        public void AfterSuite()
        {
            output += "AfterSuite";
        }

        public string GetOutput() => output;
    }
}
