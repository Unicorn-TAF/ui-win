using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BugAttribute : Attribute
    {
        public BugAttribute(string bug)
        {
            this.Bug = bug;
        }

        public string Bug { get; protected set; }
    }
}
