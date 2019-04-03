using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(string testMethod)
        {
            this.TestMethod = testMethod;
        }

        public string TestMethod { get; protected set; }
    }
}
