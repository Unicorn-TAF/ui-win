using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using ProjectSpecific.BO;
using System;

namespace ProjectSpecific.Steps
{
    public class TestingSteps : TestSteps
    {

        [TestStep("First Test Step")]
        public void FirstTestStep()
        {
            ReportStep();
            Logger.Instance.Info("");
        }

        [TestStep("Second Test Step '{0}'")]
        public void SecondTestStep(string value)
        {
            ReportStep(value);
            Logger.Instance.Info("");

        }

        [TestStep("Third Test Step '{0}'")]
        public int ThirdTestStep(int a)
        {
            ReportStep(a);
            Logger.Instance.Info("");
            return a;
        }

        [TestStep("Fourth Test Step '{0}'")]
        public void FourthTestStep(SampleObject a)
        {
            ReportStep(a);
            Logger.Instance.Info("");
        }


        [TestStep("Step which always fail '{0}'")]
        public void StepWhichSouldFail(SampleObject a)
        {
            ReportStep(a);
            Logger.Instance.Info("");
            throw new Exception("Looks strange, that step which should fail really failed");
        }
    }
}
