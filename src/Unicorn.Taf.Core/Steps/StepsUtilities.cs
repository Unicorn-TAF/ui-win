using System.Collections;
using System.Reflection;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// Provides test steps with additional functionality.
    /// </summary>
    public static class StepsUtilities
    {
        /// <summary>
        /// Get text description of test step based on title provided in <see cref="StepAttribute"/> and method argument.
        /// </summary>
        /// <param name="method"><see cref="MethodBase"/> representing test step</param>
        /// <param name="arguments">test step method arguments array</param>
        /// <returns>step description as string</returns>
        public static string GetStepInfo(MethodBase method, object[] arguments)
        {
            var attribute = method.GetCustomAttribute(typeof(StepAttribute), true) as StepAttribute;
            return attribute == null ? string.Empty : string.Format(attribute.Description, ConvertArguments(arguments));
        }

        private static string[] ConvertArguments(object[] arguments)
        {
            var convertedArguments = new string[arguments.Length];

            for (int i = 0; i < arguments.Length; i++)
            {
                convertedArguments[i] = GetArgumentValue(arguments[i]);
            }

            return convertedArguments;
        }

        private static string GetArgumentValue(object argument)
        {
            if (argument == null)
            {
                return "<null>";
            }

            var collectionArgument = argument as ICollection;

            if (collectionArgument != null)
            {
                var arrayList = new ArrayList();

                foreach (var item in collectionArgument)
                {
                    arrayList.Add(item);
                }

                return $"[{string.Join("; ", arrayList.ToArray())}]";
            }

            return argument.ToString();
        }
    }
}
