using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
        public TestAttribute()
        {
            this.Title = string.Empty;
        }

        public TestAttribute(string title)
        {
            this.Title = title;
        }

        public string Title { get; protected set; }
    }
}
