using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestDataAttribute : Attribute
    {
        public TestDataAttribute(string method)
        {
            this.Method = method;
        }

        public string Method { get; protected set; }
    }
}
