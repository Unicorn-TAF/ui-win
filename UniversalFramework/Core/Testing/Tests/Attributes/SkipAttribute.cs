using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SkipAttribute : Attribute
    {
    }
}
