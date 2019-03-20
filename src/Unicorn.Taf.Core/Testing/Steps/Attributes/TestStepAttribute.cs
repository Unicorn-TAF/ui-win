using System;

namespace Unicorn.Taf.Core.Testing.Steps.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestStepAttribute : Attribute
    {
        public TestStepAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; protected set; }
    }
}
