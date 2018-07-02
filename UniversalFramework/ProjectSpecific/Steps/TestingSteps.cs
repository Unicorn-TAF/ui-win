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
            Logger.Instance.Log(LogLevel.Info, string.Empty);
        }

        [TestStep("Say '{0}'")]
        public void Say(string value)
        {
            Logger.Instance.Log(LogLevel.Info, $"saying: '{value}'");
        }

        [TestStep("Return value '{0}'")]
        public int ReturnValue(int a)
        {
            Logger.Instance.Log(LogLevel.Info, a.ToString());
            return a;
        }

        [TestStep("Process '{0}'")]
        public void ProcessTestObject(SampleObject a)
        {
            Logger.Instance.Log(LogLevel.Info, $"retrieved {a}");
        }

        [TestStep("Step which always fail '{0}'")]
        public void StepWhichSouldFail(SampleObject a)
        {
            Logger.Instance.Log(LogLevel.Info, string.Empty);
            throw new Exception("Looks strange, that step which should fail really failed");
        }
    }
}
