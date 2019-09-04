using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DisabledAttribute : Attribute
    {
        public DisabledAttribute(string reason)
        {
            this.Reason = reason;
        }

        public string Reason { get; protected set; }
    }
}
