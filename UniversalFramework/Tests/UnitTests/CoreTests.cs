using Core.Testing;
using NUnit.Framework;
using ProjectSpecific.BO;
using ProjectSpecific.Steps;

namespace Tests.UnitTests
{
    class CoreTests : NUnitTestRunner
    {
        SampleSteps steps = new SampleSteps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void DebugTest()
        {
            //Execute(() => steps.FirstTestStep());
            //steps.Execute(() => steps.SecondTestStep("value"));
            Execute(() => steps.ThirdTestStep(3));
            Execute(() => steps.FourthTestStep(new SampleObject()));
            //steps.FirstTestStep();
            //steps.SecondTestStep("value");
            //steps.ThirdTestStep(3);
            //steps.FourthTestStep(new SampleObject());
        }


    }
}
