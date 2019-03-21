using System;

namespace Unicorn.Taf.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SuiteAttribute : Attribute
    {
        public SuiteAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; protected set; }
    }
}
