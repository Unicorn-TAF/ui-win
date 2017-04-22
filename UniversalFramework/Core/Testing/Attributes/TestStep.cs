using System;

namespace Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TestStep : Attribute
    {
        private string description;

        public TestStep(string description)
        {
            this.description = description;
        }

        public string Description
        {
            get
            {
                return description;
            }
        }
    }
}
