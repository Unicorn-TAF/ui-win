using System;
//using AspectInjector.Broker;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Steps
{
    //[Inject(typeof(StepsEvents))]
    public class TestingSteps
    {
        [Step("First Test Step")]
        public void FirstTestStep()
        {
            Logger.Instance.Log(LogLevel.Info, string.Empty);
        }

        [Step("Say '{0}'")]
        public void Say(string value)
        {
            Logger.Instance.Log(LogLevel.Info, $"saying: '{value}'");
        }

        [Step("Return value '{0}'")]
        public int ReturnValue(int a)
        {
            Logger.Instance.Log(LogLevel.Info, a.ToString());
            return a;
        }

        [Step("Process '{0}'")]
        public void ProcessTestObject(SampleObject a)
        {
            Logger.Instance.Log(LogLevel.Info, $"retrieved {a}");
        }

        [Step("Step which always fail '{0}'")]
        public void StepWhichSouldFail(SampleObject a)
        {
            Logger.Instance.Log(LogLevel.Info, string.Empty);
            throw new Exception("Looks strange, that step which should fail really failed");
        }
    }
}
