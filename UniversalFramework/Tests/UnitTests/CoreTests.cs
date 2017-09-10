using NUnit.Framework;
using ProjectSpecific;
using ProjectSpecific.Steps;
using ProjectSpecific.BO;
using System.IO;
using System.Reflection;
using System.Text;

namespace Tests.UnitTests
{
    class CoreTests : NUnitTestRunner
    {
        SampleSteps steps = new SampleSteps();

        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test For check logging")]
        public void StepsReportingTest()
        {
            string checkString = "|\t\tSTEP: Third Test Step '3'\r\n|\t\t\r\n|\t\tSTEP: Fourth Test Step 'complex object with param a = 12'\r\n|\t\t\r\n|\t\tSTEP: First Test Step\r\n|\t\t\r\n|\t\tSTEP: Second Test Step 'value'\r\n|\t\t\r\n";
            steps.ThirdTestStep(3);
            steps.FourthTestStep(new SampleObject());
            steps.FirstTestStep();
            steps.SecondTestStep("value");

            TextWriter tout = TestContext.Out;
            TextWriter tout1 = (TextWriter)tout.GetType().GetField("_out", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout);
            StringBuilder sb = (StringBuilder)tout1.GetType().GetField("_sb", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(tout1);
            Assert.That(sb.ToString(), Is.EqualTo(checkString));
        }
    }
}
