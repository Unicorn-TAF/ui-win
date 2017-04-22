using Core.Logging;
using NUnit.Framework;
using ProjectSpecific.BO;
using ProjectSpecific.Steps;

namespace Tests.UnitTests
{
    class CoreTests
    {
        SampleSteps steps = new SampleSteps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void DebugTest()
        {
            Logger.Info("simple text");
            Logger.Debug("simple text debug");
            Logger.Info("text with parameters: first parameter '{0}', second - '{1}'", "qwerty", 23.ToString());
            Logger.Debug("debug of text with parameters: first parameter '{0}', second - '{1}'", "qwerty", 23.ToString());

            steps.FirstTestStep();
            steps.SecondTestStep("value");
            steps.ThirdTestStep(3);
            steps.FourthTestStep(new SampleObject());
        }


    }
}
