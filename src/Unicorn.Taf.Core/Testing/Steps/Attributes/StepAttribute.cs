using System;

namespace Unicorn.Taf.Core.Testing.Steps.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class StepAttribute : Attribute
    {
        public StepAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; protected set; }
    }
}
