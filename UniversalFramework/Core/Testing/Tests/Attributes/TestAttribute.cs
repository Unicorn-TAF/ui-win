using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestAttribute : Attribute
    {
        private string _description;


        public TestAttribute()
        {
            _description = "";
        }

        public TestAttribute(string description)
        {
            _description = description;
        }


        public string Description
        {
            get
            {
                return _description;
            }
        }
    }
}
