using System;
using System.Runtime.CompilerServices;

namespace Unicorn.Taf.Core.Testing.Attributes
{
    /// <summary>
    /// Provides with ability to mark specified methods as tests.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TestAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class without title.
        /// </summary>
        /// <param name="order">order of test as a number (default: test line number)</param>
        public TestAttribute([CallerLineNumber] int order = 0)
        {
            Title = string.Empty;
            Order = order;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestAttribute"/> class with specified title.
        /// </summary>
        /// <param name="title">test title</param>
        /// <param name="order">order of test as a number (default: test line number)</param>
        public TestAttribute(string title, [CallerLineNumber] int order = 0)
        {
            Title = title;
            Order = order;
        }

        /// <summary>
        /// Gets test title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets test order.
        /// </summary>
        public int Order { get; }
    }
}
