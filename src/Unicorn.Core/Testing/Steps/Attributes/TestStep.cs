using System;

namespace Unicorn.Core.Testing.Steps.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
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
                return this.description;
            }
        }
    }
}
