using System;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteParametersSet
    {
        public string SetName;
        public object[] Parameters;

        public TestSuiteParametersSet(string setName, params object[] parameters)
        {
            SetName = setName;
            Parameters = new object[parameters.Length];
            Array.Copy(parameters, Parameters, parameters.Length);
        }
    }
}
