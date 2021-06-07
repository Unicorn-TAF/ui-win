using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified class in assembly which contains 
    /// methods for launch initialization and finalization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class TestAssemblyAttribute : Attribute
    {
    }
}
