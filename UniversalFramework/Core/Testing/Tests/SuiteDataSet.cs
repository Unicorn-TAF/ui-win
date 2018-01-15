using System;

namespace Unicorn.Core.Testing.Tests
{
    public class SuiteDataSet
    {
        private string name;
        private object[] parameters;

        public SuiteDataSet(string name, params object[] parameters)
        {
            this.name = name;
            this.parameters = new object[parameters.Length];
            Array.Copy(parameters, this.Parameters, parameters.Length);
        }

        public string Name => this.name;

        public object[] Parameters => this.parameters;
    }
}
