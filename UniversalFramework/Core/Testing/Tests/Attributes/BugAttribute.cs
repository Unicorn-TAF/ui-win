using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BugAttribute : Attribute
    {
        private string bug;

        public BugAttribute(string bug)
        {
            this.bug = bug;
        }

        public string Bug
        {
            get
            {
                return bug;
            }
        }
    }
}
