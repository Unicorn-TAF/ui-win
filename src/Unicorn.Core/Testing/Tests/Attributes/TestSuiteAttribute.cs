using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        private string name;

        public TestSuiteAttribute(string name)
        {
            this.name = name;
        }

        public string Name => this.name;
    }
}
