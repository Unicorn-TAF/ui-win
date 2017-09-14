using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BugAttribute : Attribute
    {
        private string _bug;

        public BugAttribute(string bug)
        {
            _bug = bug;
        }

        public string Bug
        {
            get
            {
                return _bug;
            }
        }
    }
}
