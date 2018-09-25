using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
        public TestAttribute()
        {
            this.Description = string.Empty;
        }

        public TestAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; protected set; }
    }
}
