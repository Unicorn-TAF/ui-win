using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestSuiteAttribute : Attribute
    {
        public TestSuiteAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; protected set; }
    }
}
