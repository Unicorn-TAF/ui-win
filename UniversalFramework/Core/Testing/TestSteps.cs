using Core.Logging;
using Core.Reporting;
using Core.Testing.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace Core.Testing
{
    public class TestSteps
    {
        protected void ReportStep(params object[] parameters)
        {
            MethodBase mb = new StackFrame(1).GetMethod();
            string descriptionTemplate = string.Empty;

            object[] attributes = mb.GetCustomAttributes(typeof(TestStep), true);

            if (attributes.Length == 0)
            {
                descriptionTemplate = mb.Name + ":";
                for (int i = 0; i < parameters.Length; i++)
                    descriptionTemplate += $" '{i}'";
            }
            else
            {
                TestStep attribute = (TestStep)attributes[0];
                descriptionTemplate = attribute.Description;
            }

            Logger.Instance.Info(string.Format(descriptionTemplate, parameters));
            Reporter.Instance.Report(string.Format(descriptionTemplate, parameters));
        }


        public TestSteps()
        {

        }


    }
}
