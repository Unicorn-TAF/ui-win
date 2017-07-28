using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.Steps;
using ProjectSpecific.BO;

namespace Tests.UnitTests
{
    class CoreTests : NUnitTestRunner
    {
        SampleSteps steps = new SampleSteps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void DebugTest()
        {
            steps.ThirdTestStep(3);
            steps.FourthTestStep(new SampleObject());
            steps.FirstTestStep();
            steps.SecondTestStep("value");
        }
    }
}
