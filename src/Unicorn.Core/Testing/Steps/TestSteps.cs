using System.Collections;
using System.Reflection;
using Unicorn.Core.Testing.Steps.Attributes;

namespace Unicorn.Core.Testing.Steps
{
    public static class TestSteps
    {
        public static string GetStepInfo(MethodBase method, object[] arguments)
        {
            var attribute = method.GetCustomAttribute(typeof(TestStepAttribute), true) as TestStepAttribute;
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
