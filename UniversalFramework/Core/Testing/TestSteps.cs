using Core.Logging;
using Core.Testing.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace Core.Testing
{
    public class TestSteps
    {
        protected void ReportMethod(MethodBase mb, params object[] parameters)
        {
            TestStep attr = (TestStep)mb.GetCustomAttributes(typeof(TestStep), true)[0];
            string template = attr.Description;
            Logger.Info(string.Format(template, parameters));
        }

        protected void ReportMethod(params object[] parameters)
        {
            StackTrace stackTrace = new StackTrace(0);

            MethodBase mb = stackTrace.GetFrame(0). GetMethod();
            Logger.Info(mb.Name);
            TestStep attr = (TestStep)mb.GetCustomAttributes(typeof(TestStep), true)[0];
            string template = attr.Description;
            Logger.Info(string.Format(template, parameters));
        }


        public TestSteps()
        {

        }


    }
}
