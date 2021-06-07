using System;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified test as dependent on some other tests within the same suite.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DependsOnAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnAttribute"/> class with specified test method name.
        /// </summary>
        /// <param name="testMethod">test method name</param>
        public DependsOnAttribute(string testMethod)
        {
            TestMethod = testMethod;
        }

        /// <summary>
        /// Gets test method name.
        /// </summary>
        public string TestMethod { get; }
    }
}
