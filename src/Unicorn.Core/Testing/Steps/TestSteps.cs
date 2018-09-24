using System.Reflection;
using System.Text;
using Unicorn.Core.Testing.Steps.Attributes;

namespace Unicorn.Core.Testing.Steps
{
    public static class TestSteps
    {
        public static string GetStepInfo(MethodBase method, object[] arguments)
        {
            var stepDescription = new StringBuilder();

            object[] attributes = method.GetCustomAttributes(typeof(TestStepAttribute), true);

            // generate description based on method signature if TestStep attribute is not defined
            // else if TestStep attribute is defined, use it as template for string.Format
            if (attributes.Length == 0)
            {
                stepDescription.Append(method.Name);

                if (arguments.Length > 0)
                {
                    stepDescription.Append(":");
                }
                    
                for (int i = 0; i < arguments.Length; i++)
                {
                    stepDescription.Append($" '{arguments[i]}'");
                }
            }
            else
            {
                TestStepAttribute attribute = (TestStepAttribute)attributes[0];
                stepDescription.AppendFormat(attribute.Description, arguments);
            }

            return stepDescription.ToString();
        }
    }
}
