using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BeforeTestAttribute : Attribute
    {
    }
}
