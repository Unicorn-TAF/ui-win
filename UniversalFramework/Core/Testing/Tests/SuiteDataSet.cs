using System;
using System.Collections.Generic;

namespace Unicorn.Core.Testing.Tests
{
    public class SuiteDataSet
    {
        private string name;
        private List<object> parameters;

        public SuiteDataSet(string name, params object[] parameters)
        {
            this.name = name;
            this.parameters = new List<object>(parameters);
        }

        public string Name => this.name;

        public List<object> Parameters => this.parameters;
    }
}
