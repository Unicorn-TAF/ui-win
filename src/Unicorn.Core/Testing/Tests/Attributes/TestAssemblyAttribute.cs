using System;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class TestAssemblyAttribute : Attribute
    {
    }
}
