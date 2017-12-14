using System;
using AspectInjector.Broker;
using ProjectSpecific.BO;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class TestingSteps : TestSteps
    {
        [TestStep("First Test Step")]
        public void FirstTestStep()
        {
            Logger.Instance.Info(string.Empty);
        }

        [TestStep("Second Test Step '{0}'")]
        public void SecondTestStep(string value)
        {
            Logger.Instance.Info(string.Empty);
        }

        [TestStep("Third Test Step '{0}'")]
        public int ThirdTestStep(int a)
        {
            Logger.Instance.Info(string.Empty);
            return a;
        }

        [TestStep("Fourth Test Step '{0}'")]
        public void FourthTestStep(SampleObject a)
        {
            Logger.Instance.Info(string.Empty);
        }

        [TestStep("Step which always fail '{0}'")]
        public void StepWhichSouldFail(SampleObject a)
        {
            Logger.Instance.Info(string.Empty);
            throw new Exception("Looks strange, that step which should fail really failed");
        }
    }
}
