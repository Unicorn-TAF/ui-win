using System;
using AspectInjector.Broker;
using Demo.Specifics.BO;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.Specifics.Steps
{
    [Inject(typeof(StepsEvents))]
    public class DemoSteps
    {
        [Step("First Test Step")]
        public void FirstTestStep() =>
            Logger.Instance.Log(LogLevel.Debug, "debug information");

        [Step("Say '{0}'")]
        public void Say(string value) =>
            Logger.Instance.Log(LogLevel.Info, $"saying: '{value}'");

        [Step("Return value '{0}'")]
        public int ReturnValue(int valueToReturn)
        {
            Logger.Instance.Log(LogLevel.Trace, valueToReturn.ToString());
            return valueToReturn;
        }

        [Step("Process '{0}'")]
        public void ProcessTestObject(DemoObject obj) =>
            Logger.Instance.Log(LogLevel.Debug, $"Processed {obj}");

        [Step("Step which always fail '{0}'")]
        public void StepWhichSouldFail(DemoObject a)
        {
            Logger.Instance.Log(LogLevel.Info, string.Empty);
            throw new NotImplementedException("Looks strange, that step which should fail really failed");
        }
    }
}
