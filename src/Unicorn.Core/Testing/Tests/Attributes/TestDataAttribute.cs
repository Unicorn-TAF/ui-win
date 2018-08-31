using System;
using System.Reflection;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestDataAttribute : Attribute
    {
        private string method;

        public TestDataAttribute(string method)
        {
            this.method = method;
        }

        public string Method => this.method;
    }
}
