using Core.Testing;
using Core.Testing.Attributes;
using ProjectSpecific.BO;
using System.Reflection;

namespace ProjectSpecific.Steps
{
    public class SampleSteps : TestSteps
    {
        [TestStep("First Test Step")]
        public void FirstTestStep()
        {
            ReportMethod(MethodBase.GetCurrentMethod());
            //ReportMethod();
        }

        [TestStep("Second Test Step '{0}'")]
        public void SecondTestStep(string value)
        {
            ReportMethod(MethodBase.GetCurrentMethod(), value);
            //ReportMethod(value);

        }

        [TestStep("Third Test Step '{0}'")]
        public int ThirdTestStep(int a)
        {
            ReportMethod(MethodBase.GetCurrentMethod(), a);
            //ReportMethod(a);
            return a;
        }

        [TestStep("Fourth Test Step '{0}'")]
        public void FourthTestStep(SampleObject a)
        {
            ReportMethod(MethodBase.GetCurrentMethod(), a);
            //ReportMethod(a);
        }
    }
}
