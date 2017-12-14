using System;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuiteParametersSet
    {
        private string name;
        private object[] parameters;

        public TestSuiteParametersSet(string name, params object[] parameters)
        {
            this.name = name;
            this.parameters = new object[parameters.Length];
            Array.Copy(parameters, this.Parameters, parameters.Length);
        }

        public string Name => this.name;

        public object[] Parameters => this.parameters;
    }
}
