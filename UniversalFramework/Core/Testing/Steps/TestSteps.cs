using Unicorn.Core.Logging;
using Unicorn.Core.Reporting;
using Unicorn.Core.Testing.Steps.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace Unicorn.Core.Testing.Steps
{
    public class TestSteps
    {
        protected void ReportStep(params object[] parameters)
        {
            MethodBase mb = new StackFrame(1).GetMethod();
            string stepDescription = string.Empty;

            object[] attributes = mb.GetCustomAttributes(typeof(TestStep), true);

            if (attributes.Length == 0)
            {
                stepDescription = mb.Name + ":";
                for (int i = 0; i < parameters.Length; i++)
                    stepDescription += $" '{parameters[i]}'";
            }
            else
            {
                TestStep attribute = (TestStep)attributes[0];
                stepDescription = attribute.Description;
                stepDescription = string.Format(stepDescription, parameters);
            }

            Logger.Instance.Info("STEP: " + stepDescription);
            Reporter.Instance.ReportInfo(stepDescription);
        }


        public TestSteps()
        {

        }


    }
}
