using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified classes as test suites.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SuiteAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuiteAttribute"/> class with specified name.
        /// </summary>
        /// <param name="name">test suite name</param>
        public SuiteAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets or sets test suite name.
        /// </summary>
        public string Name { get; protected set; }
    }
}
