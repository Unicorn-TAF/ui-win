using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
        private string description;

        public TestAttribute()
        {
            description = string.Empty;
        }

        public TestAttribute(string description)
        {
            this.description = description;
        }

        public string Description => this.description;
    }
}
