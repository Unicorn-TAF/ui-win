using System;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteParametersSet
    {
        public string Name;
        public object[] Parameters;

        public TestSuiteParametersSet(string setName, params object[] parameters)
        {
            this.Name = setName;
            this.Parameters = new object[parameters.Length];
            Array.Copy(parameters, this.Parameters, parameters.Length);
        }
    }
}
