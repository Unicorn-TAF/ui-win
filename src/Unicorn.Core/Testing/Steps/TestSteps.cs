using System.Reflection;
using Unicorn.Core.Testing.Steps.Attributes;

namespace Unicorn.Core.Testing.Steps
{
    public class TestSteps
    {
        public static string GetStepInfo(MethodBase method, object[] arguments)
        {
            string stepDescription = string.Empty;

            object[] attributes = method.GetCustomAttributes(typeof(TestStep), true);

            // generate description based on method signature if TestStep attribute is not defined
            // else if TestStep attribute is defined, use it as template for string.Format
            if (attributes.Length == 0)
            {
                stepDescription = method.Name;

                if (arguments.Length > 0)
                {
                    stepDescription += ":";
                }
                    
                for (int i = 0; i < arguments.Length; i++)
                {
                    stepDescription += $" '{arguments[i]}'";
                }
            }
            else
            {
                TestStep attribute = (TestStep)attributes[0];
                stepDescription = attribute.Description;
                stepDescription = string.Format(stepDescription, arguments);
            }

            return stepDescription;
        }
    }
}
